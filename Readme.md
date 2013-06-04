SnagScript
=========
*A JavaScript interpreter library written in C#*


SnagScript started as a project to port the Java based ZemScript interpreter by Cameron Zemek to C#.

Once the port was completed I decided to try and adapt the interpreter to run JavaScript code.

I have included an example interpreter console, and a sample script which trys to display most of the abilities of the interpreter.

* * *

SnagScript roughly supports:
------------------------------------
* Some structures and program flow
    * try/catch/finally
    * if/then/else
    * while loop
    * for loop
* Basic support for functions
* Constructors / Classes
* Basic Datatypes
    * Strings
    * Integers
    * Floats
    * Booleans
    * Arrays
* Basic operators
    * <, >, ==, !=, <=, >=
    * +, ++, +=, -, --, -=
    * ||, &&
    * /, *, %, ^
    * |, &, !
    * \

* * *

TODO
---------
*In no particular order*

* Prototypes
* Full support for all operators
    * === vs. ==
    * x++ vs. ++x
    * (string litural) += x
* Full property support
    * Descriptors
    * Enumeration
* Hoisting
* Built in classes
    * Object()
    * String()

* * *
    SnagScript is based off the Java based script interpreter 'zemscript' written by Cameron Zemek. https://code.google.com/p/zemscript/