/*
 * Copyright (c) 2008 Cameron Zemek
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */
using SnagScript.ParserNodes;
using System.Collections.Generic;
using System;
using SnagScript.BuiltInTypes;


namespace SnagScript
{

    /**
     * Check the syntax and convert the Token stream into Abstract Syntax Tree.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class Parser
    {
        // Look ahead buffer for reading tokens from the lexer
        TokenBuffer lookAheadBuffer;

        public Parser(Lexer lexer)
        {
            lookAheadBuffer = new TokenBuffer(lexer, 3);
        }

        private TokenType LookAhead(int i)
        {
            if (lookAheadBuffer.IsEmpty || i > lookAheadBuffer.Count)
            {
                return TokenType.NONE; // EOF
            }
            Token token = lookAheadBuffer.GetToken(i - 1); // 1-based index
            return token.Type;
        }

        private Token Match(TokenType tokenType)
        {
            Token token = lookAheadBuffer.ReadToken();
            if (token == null)
            {
                throw new ParserException("Expecting type " + tokenType + " but didn't get a token");
            }
            if (token.Type != tokenType)
            {
                throw new ParserException("Expecting type " + tokenType + " but got " + token.Type, token.Position);
            }
            return token;
        }

        public ScriptExecutable Parse()
        {
            List<Node> script = new List<Node>();
            while (LookAhead(1) != TokenType.NONE)
            {
                script.Add(Statement());
            }
            return new ScriptExecutable(script);
        }

        private BlockNode Block()
        {
            // LBRACE! statement* RBRACE!
            Token lbrace = Match(TokenType.LBRACE);
            List<Node> block = new List<Node>();
            while (LookAhead(1) != TokenType.RBRACE)
            {
                block.Add(Statement());
            }
            Match(TokenType.RBRACE);
            return new BlockNode(lbrace.Position, block);
        }

        private Node Statement()
        {
            // | (ID LPAREN) => functionCall  END_STATEMENT
            // | VARIABLE! ASSIGN! expression END_STATEMENT
            // | RETURN expression END_STATEMENT
            // | IF | WHILE | FOR_EACH
            TokenType type = LookAhead(1);
            if (type == TokenType.VARIABLE && LookAhead(2) == TokenType.LPAREN)
            {
                Node funcCall = FunctionCall();
                Match(TokenType.END_STATEMENT);
                return funcCall;
            }
            else if (type == TokenType.VARIABLE || type == TokenType.VAR)
            {
                return VariableDeclaration();
            }
            else if (type == TokenType.RETURN)
            {
                SourcePosition pos = Match(TokenType.RETURN).Position;
                Node expression = Expression();
                Match(TokenType.END_STATEMENT);
                return new ReturnNode(pos, expression);
            }
            else if (type == TokenType.IF)
            {
                return If();
            }
            else if (type == TokenType.WHILE)
            {
                return While();
            }
            else if (type == TokenType.FOR_EACH)
            {
                return Foreach();
            }
            else if (type == TokenType.FUNCTION)
            {
                return FunctionDeclaration();
            }
            else if (type == TokenType.THIS)
            {
                return This();
            }
            else if (type == TokenType.TRY)
            {
                return TryCatch();
            }
            else
            {
                // We only get here if there is token from the lexer
                // that is not handled by parser yet.
                throw new ParserException("Unknown token type " + type);
            }
        }

        private Node TryCatch()
        {
            // IF! condition block else?
            SourcePosition pos = Match(TokenType.TRY).Position;

            BlockNode tryBlock = Block();

            CatchNode catchNode = null;
            if (LookAhead(1) == TokenType.CATCH)
            {
                catchNode = Catch();
            }

            BlockNode finallyBlock = null;
            if (LookAhead(1) == TokenType.FINALLY)
            {
                finallyBlock = Finally();
            }

            if (catchNode != null || finallyBlock != null)
                return new TryCatchFinallyNode(pos, tryBlock, catchNode, finallyBlock);

            throw new ParserException("Expected catch or finally in try catch statement", pos);
        }

        private BlockNode Finally()
        {
            Match(TokenType.FINALLY);
            return Block();
        }

        private CatchNode Catch()
        {
            SourcePosition pos = Match(TokenType.CATCH).Position;

            VariableNode errorVariable = null;
            if (LookAhead(1) == TokenType.LPAREN)
            {
                Match(TokenType.LPAREN);
                Token variable = Match(TokenType.VARIABLE);
                errorVariable = new VariableNode(variable.Position, variable.Text);
                Match(TokenType.RPAREN);
            }

            BlockNode catchBlock = Block();

            return new CatchNode(pos, errorVariable, catchBlock);
        }

        private Node Property()
        {
            if (LookAhead(2) == TokenType.LPAREN)
            {
                return FunctionCall();
            }
            else
            {
                Token varToken = Match(TokenType.VARIABLE);
                if (LookAhead(1) == TokenType.PERIOD)
                {
                    Match(TokenType.PERIOD);
                    Node propNode = new PropertyNode(varToken.Position, varToken.Text);
                    return new PropertyTreeNode(propNode.Position, propNode, Property());
                }
                else if (LookAhead(1) == TokenType.ASSIGN)
                {
                    Match(TokenType.ASSIGN);
                    Node value = Expression();
                    PropertyNode propNode = new PropertyNode(varToken.Position, varToken.Text);
                    return new AssignNode(propNode.Position, propNode, value, true);
                }
                else
                {
                    return new PropertyNode(varToken.Position, varToken.Text);
                }
            }
        }

        private Node VariableDeclaration()
        {
            bool local = false;
            if (LookAhead(1) == TokenType.VAR)
            {
                Match(TokenType.VAR);
                local = true;
            }

            Node result = Variable();

            if (LookAhead(1) == TokenType.ASSIGN)
            {
                SourcePosition pos = Match(TokenType.ASSIGN).Position;
                Node value = Expression();
                result = new AssignNode(pos, result, value, local);
            }

            if (LookAhead(1) == TokenType.END_STATEMENT)
            {
                Match(TokenType.END_STATEMENT);
            }

            return result;
        }

        private Node Condition()
        {
            Match(TokenType.LPAREN);
            Node test = BooleanExpression();
            Match(TokenType.RPAREN);
            return test;
        }

        private Node If()
        {
            // IF! condition block else?
            SourcePosition pos = Match(TokenType.IF).Position;
            Node test = Condition();
            BlockNode thenBlock = Block();
            Node elseBlock = null;
            if (LookAhead(1) == TokenType.ELSE)
            {
                elseBlock = Else();
            }
            return new IfNode(pos, test, thenBlock, elseBlock);
        }

        private Node Else()
        {
            // ELSE! (if | block)!
            Match(TokenType.ELSE);
            if (LookAhead(1) == TokenType.IF)
            {
                return If();
            }
            else
            {
                return Block();
            }
        }

        private Node While()
        {
            // WHILE! condition block
            SourcePosition pos = Match(TokenType.WHILE).Position;
            Node test = Condition();
            Node loopBlock = Block();
            return new WhileNode(pos, test, loopBlock);
        }

        private Node Foreach()
        {
            // FOREACH! LPAREN! VARIABLE! AS! VARIABLE! (^COLON VARIABLE!) RPAREN!
            // LBRACE! block RBRACE!
            SourcePosition pos = Match(TokenType.FOR_EACH).Position;
            Match(TokenType.LPAREN);
            Token t = Match(TokenType.VARIABLE);
            VariableNode onEach = new VariableNode(t.Position, t.Text);
            Match(TokenType.AS);
            t = Match(TokenType.VARIABLE);
            VariableNode value = new VariableNode(t.Position, t.Text);
            Node asValue = value;
            if (LookAhead(1) == TokenType.COLON)
            {
                SourcePosition entryPos = Match(TokenType.COLON).Position;
                VariableNode key = value;
                t = Match(TokenType.VARIABLE);
                value = new VariableNode(t.Position, t.Text);
                //asValue = new ZemDictionaryEntryNode(entryPos, key, value);
            }
            Match(TokenType.RPAREN);
            Node loopBlock = Block();
            return new ForeachNode(pos, onEach, asValue, loopBlock);
        }

        private Node Array()
        {
            // LBRACKET! (expression (COMMA^ expression)*)? RBRACKET!
            SourcePosition pos = Match(TokenType.LBRACKET).Position;
            List<Node> elements = new List<Node>();
            if (LookAhead(1) != TokenType.RBRACKET)
            {
                elements.Add(Expression());
                while (LookAhead(1) == TokenType.COMMA)
                {
                    Match(TokenType.COMMA);
                    elements.Add(Expression());
                }
            }
            Match(TokenType.RBRACKET);
            return new ArrayNode(pos, elements);
        }

        private Node Object()
        {
            SourcePosition pos = Match(TokenType.LBRACE).Position;
            ObjectNode objNode = new ObjectNode(pos);
            while (LookAhead(1) != TokenType.RBRACE)
            {
                Token property = Match(TokenType.VARIABLE);
                Match(TokenType.COLON);
                Node value = Expression();
                objNode.SetProperty(property.Text, value);
                if (LookAhead(1) == TokenType.COMMA)
                    Match(TokenType.COMMA);
            }
            Match(TokenType.RBRACE);
            return objNode;
        }

        private Node Key()
        {
            // STRING_LITERAL | NUMBER
            if (LookAhead(1) == TokenType.STRING_LITERAL)
            {
                Token t = Match(TokenType.STRING_LITERAL);
                return new StringNode(t.Position, t.Text);
            }
            else
            {
                Token t = Match(TokenType.INTEGER);
                return new IntegerNode(t.Position, t.Text);
            }
        }

        private Node FunctionDeclaration()
        {
            // FUNCTION! VARIABLE! LPAREN! parameterList? RPAREN!
            // LBRACE! block() RBRACE!
            SourcePosition pos = Match(TokenType.FUNCTION).Position;

            String functionName = "";

            if (LookAhead(1) == TokenType.VARIABLE)
            {
                functionName = Match(TokenType.VARIABLE).Text;
            }

            Match(TokenType.LPAREN);

            List<Node> paramList = FunctionDeclarationNode.NO_PARAMETERS;
            if (LookAhead(1) != TokenType.RPAREN)
            {
                paramList = ParameterList();
            }

            Match(TokenType.RPAREN);

            Node body = Block();

            return new FunctionDeclarationNode(pos, paramList, functionName, body);
        }

        private List<Node> ParameterList()
        {
            // (parameter (COMMA! parameter)* )?
            List<Node> parameters = new List<Node>();
            parameters.Add(Parameter());
            while (LookAhead(1) == TokenType.COMMA)
            {
                Match(TokenType.COMMA);
                parameters.Add(Parameter());
            }
            return parameters;
        }

        private Node Parameter()
        {
            // variable (ASSIGN^ expression)?
            Token t = Match(TokenType.VARIABLE);
            VariableNode variable = new VariableNode(t.Position, t.Text);
            if (LookAhead(1) == TokenType.ASSIGN)
            {
                SourcePosition pos = Match(TokenType.ASSIGN).Position;
                Node e = Expression();
                return new AssignNode(pos, variable, e, true);
            }
            return variable;
        }

        private Node Expression()
        {
            TokenType type = LookAhead(1);
            if (type == TokenType.LBRACKET)
            {
                return Array();
            }
            else if (type == TokenType.LBRACE)
            {
                return Object();
            }
            else if (type == TokenType.FUNCTION)
            {
                //return FunctionExpression();
                return FunctionDeclaration();
            }
            else if (type == TokenType.NEW)
            {
                return New();
            }
            else
            {
                // An expression can result in a string, boolean or number
                return StringExpression();
            }
        }

        private Node This()
        {
            SourcePosition pos = Match(TokenType.THIS).Position;
            Match(TokenType.PERIOD);
            Node property = Property();
            if (LookAhead(1) == TokenType.END_STATEMENT)
            {
                Match(TokenType.END_STATEMENT);
            }
            return new ThisNode(pos, property);
        }

        private Node New()
        {
            Match(TokenType.NEW);

            Token constructorToken = Match(TokenType.VARIABLE);

            List<Node> arguments = FunctionCallNode.NO_ARGUMENTS;
            if (LookAhead(1) == TokenType.LPAREN)
            {
                Match(TokenType.LPAREN);
                if (LookAhead(1) != TokenType.RPAREN)
                {
                    arguments = ArgumentList();
                }
                Match(TokenType.RPAREN);
            }

            return new NewObjectNode(constructorToken.Position, constructorToken.Text, arguments);
        }

        private Node SumExpression()
        {
            // term ((PLUS^|MINUS^) term)*
            Node termExpression = Term();
            while (LookAhead(1) == TokenType.PLUS ||
                    LookAhead(1) == TokenType.MINUS)
            {
                if (LookAhead(1) == TokenType.PLUS)
                {
                    termExpression = new AddOpNode(Match(TokenType.PLUS).Position,
                        termExpression, Term());
                }
                else if (LookAhead(1) == TokenType.MINUS)
                {
                    termExpression = new SubtractOpNode(Match(TokenType.MINUS).Position,
                        termExpression, Term());
                }
            }
            return termExpression;
        }

        private Node Term()
        {
            // factor ((MUL^|DIV^|MOD^) factor)*
            Node factorExpression = Factor();
            while (LookAhead(1) == TokenType.MULTIPLY ||
                    LookAhead(1) == TokenType.DIVIDE ||
                    LookAhead(1) == TokenType.MOD)
            {
                if (LookAhead(1) == TokenType.MULTIPLY)
                {
                    factorExpression = new MultiplyOpNode(
                        Match(TokenType.MULTIPLY).Position,
                        factorExpression, Factor());
                }
                else if (LookAhead(1) == TokenType.DIVIDE)
                {
                    factorExpression = new DivideOpNode(
                        Match(TokenType.DIVIDE).Position,
                        factorExpression, Factor());
                }
                else if (LookAhead(1) == TokenType.MOD)
                {
                    factorExpression = new ModOpNode(
                        Match(TokenType.MOD).Position,
                        factorExpression, Factor());
                }
            }
            return factorExpression;
        }

        private Node Factor()
        {
            // signExpr (POW^ signExpr)*
            Node expression = SignExpression();
            while (LookAhead(1) == TokenType.POWER)
            {
                expression = new PowerOpNode(Match(TokenType.POWER).Position,
                    expression, SignExpression());
            }
            return expression;
        }

        private Node SignExpression()
        {
            // (MINUS^|PLUS^)? value
            Token signToken = null;
            if (LookAhead(1) == TokenType.MINUS)
            {
                signToken = Match(TokenType.MINUS);
            }
            else if (LookAhead(1) == TokenType.PLUS)
            {
                Match(TokenType.PLUS);
            }
            Node value = Value();
            if (signToken != null)
            {
                return new NegateOpNode(signToken.Position, value);
            }
            else
            {
                return value;
            }
        }

        private Node Value()
        {
            // (ID LPAREN) => functionCall | atom
            if (LookAhead(1) == TokenType.VARIABLE && LookAhead(2) == TokenType.LPAREN)
            {
                return FunctionCall();
            }
            else
            {
                return Atom();
            }
        }

        private Node FunctionCall()
        {
            // f:ID^ LPAREN! argumentList RPAREN!
            Token functionToken = Match(TokenType.VARIABLE);
            String functionName = functionToken.Text;
            Match(TokenType.LPAREN);
            List<Node> arguments = FunctionCallNode.NO_ARGUMENTS;
            if (LookAhead(1) != TokenType.RPAREN)
            {
                arguments = ArgumentList();
            }
            Match(TokenType.RPAREN);
            return new FunctionCallNode(functionToken.Position, functionName, arguments);
        }

        private List<Node> ArgumentList()
        {
            // (expression (COMMA! expression)* )?
            List<Node> arguments = new List<Node>();
            arguments.Add(Expression());
            while (LookAhead(1) == TokenType.COMMA)
            {
                Match(TokenType.COMMA);
                arguments.Add(Expression());
            }
            return arguments;
        }

        private Node Atom()
        {
            // NUMBER
            // | TRUE | FALSE
            // | LPAREN^ sumExpr RPAREN!
            // | variable
            TokenType type = LookAhead(1);
            if (type == TokenType.INTEGER)
            {
                Token t = Match(TokenType.INTEGER);
                return new IntegerNode(t.Position, t.Text);
            }
            else if (type == TokenType.DECIMAL)
            {
                Token t = Match(TokenType.DECIMAL);
                return new FloatNode(t.Position, t.Text);
            }
            else if (type == TokenType.TRUE)
            {
                return new TrueNode(Match(TokenType.TRUE).Position);
            }
            else if (type == TokenType.FALSE)
            {
                return new FalseNode(Match(TokenType.FALSE).Position);
            }
            else if (type == TokenType.LPAREN)
            {
                Match(TokenType.LPAREN);
                Node atom = Expression();
                Match(TokenType.RPAREN);
                return atom;
            }
            else if (type == TokenType.THIS)
            {
                return This();
            }
            else
            {
                return Variable();
            }
        }

        private Node Variable()
        {
            // VARIABLE_DEF VARIABLE ASSIGN
            Token variable = Match(TokenType.VARIABLE);
            Node varNode = new VariableNode(variable.Position, variable.Text);
            TokenType next = LookAhead(1);
            if (next == TokenType.LBRACKET)
            {
                SourcePosition pos = Match(TokenType.LBRACKET).Position;
                Node key = Expression();
                Match(TokenType.RBRACKET);
                return new LookupNode(pos, (VariableNode)varNode, key);
            }
            else if (LookAhead(1) == TokenType.PERIOD)
            {
                Match(TokenType.PERIOD);
                PropertyTreeNode propertyTree = new PropertyTreeNode(varNode.Position, varNode, Property());
                if (LookAhead(1) == TokenType.END_STATEMENT)
                {
                    Match(TokenType.END_STATEMENT);
                }
                return propertyTree;
            }
            else
            {
                return varNode;
            }
        }

        private Node BooleanExpression()
        {
            // booleanTerm (OR^ booleanExpression)?
            Node boolTerm = BooleanTerm();
            if (LookAhead(1) == TokenType.OR)
            {
                return new OrOpNode(Match(TokenType.OR).Position, boolTerm, BooleanExpression());
            }
            return boolTerm;
        }

        private Node BooleanTerm()
        {
            // booleanFactor (AND^ booleanTerm)?
            Node boolFactor = BooleanFactor();
            if (LookAhead(1) == TokenType.AND)
            {
                return new AndOpNode(Match(TokenType.AND).Position, boolFactor, BooleanTerm());
            }
            return boolFactor;
        }

        private Node BooleanFactor()
        {
            // (NOT^)? relation
            if (LookAhead(1) == TokenType.NOT)
            {
                return new NotOpNode(Match(TokenType.NOT).Position, BooleanRelation());
            }
            return BooleanRelation();
        }

        private Node BooleanRelation()
        {
            // sumExpr ((LE^ | LT^ | GE^ | GT^ | EQUAL^ | NOT_EQUAL^) sumExpr)?
            Node sumExpr = SumExpression();
            TokenType type = LookAhead(1);
            if (type == TokenType.LESS_EQUAL)
            {
                return new LessEqualOpNode(Match(TokenType.LESS_EQUAL).Position,
                    sumExpr, SumExpression());
            }
            else if (type == TokenType.LESS_THEN)
            {
                return new LessThenOpNode(Match(TokenType.LESS_THEN).Position,
                    sumExpr, SumExpression());
            }
            else if (type == TokenType.GREATER_EQUAL)
            {
                return new GreaterEqualOpNode(Match(TokenType.GREATER_EQUAL).Position,
                    sumExpr, SumExpression());
            }
            else if (type == TokenType.GREATER_THEN)
            {
                return new GreaterThenOpNode(Match(TokenType.GREATER_THEN).Position,
                    sumExpr, SumExpression());
            }
            else if (type == TokenType.EQUAL)
            {
                return new EqualsOpNode(Match(TokenType.EQUAL).Position,
                    sumExpr, SumExpression());
            }
            else if (type == TokenType.NOT_EQUAL)
            {
                return new NotEqualsOpNode(Match(TokenType.NOT_EQUAL).Position,
                    sumExpr, SumExpression());
            }
            return sumExpr;
        }

        private Node StringExpression()
        {
            // string (CONC^ stringExpr)?
            Node stringNode = String();
            if (LookAhead(1) == TokenType.PLUS)
            {
                return new ConcatOpNode(Match(TokenType.PLUS).Position,
                    stringNode, StringExpression());
            }
            return stringNode;
        }

        private Node String()
        {
            // STRING_LITERAL | boolExpr
            if (LookAhead(1) == TokenType.STRING_LITERAL)
            {
                Token t = Match(TokenType.STRING_LITERAL);
                return new StringNode(t.Position, t.Text);
            }
            else if (LookAhead(2) == TokenType.PLUS && LookAhead(3) == TokenType.STRING_LITERAL)
            {
                return Atom();
            }
            else
            {
                return BooleanExpression();
            }
        }
    }
}