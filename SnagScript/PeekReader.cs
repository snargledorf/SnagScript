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
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace SnagScript
{

    /**
     * Wraps a Reader to provide peeking into the character stream.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class PeekReader
    {
        private Stream inStream;
        private List<byte> peekBuffer;

        public PeekReader(Stream inStream, int peekLimit)
        {
            this.inStream = inStream;
            peekBuffer = new List<byte>(peekLimit);
            FillPeekBuffer();
        }

        public void Close()
        {
            inStream.Close();
        }

        private void FillPeekBuffer()
        {
            peekBuffer.Clear();
            long pos = inStream.Position;
            byte[] buffer = new byte[peekBuffer.Capacity];
            inStream.Read(buffer, 0, buffer.Length);
            inStream.Position = pos;
            peekBuffer.AddRange(buffer);
        }

        public int Read()
        {
            int c = inStream.ReadByte();
            FillPeekBuffer();
            return c;
        }

        /**
         * Return a character that is further in the stream.
         *
         * @param lookAhead How far to look into the stream.
         * @return Character that is lookAhead characters into the stream.
         */
        public int Peek(int lookAhead)
        {
            if (lookAhead < 1 || lookAhead > peekBuffer.Capacity)
            {
                throw new IndexOutOfRangeException("lookAhead must be between 1 and " + peekBuffer.Capacity);
            }
            if (lookAhead > peekBuffer.Count)
            {
                return -1;
            }
            return peekBuffer[lookAhead - 1];
        }
    }
}