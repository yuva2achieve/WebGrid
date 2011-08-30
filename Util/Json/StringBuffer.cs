﻿/*
Copyright ©  Olav Christian Botterli.

Dual licensed under the MIT or GPL Version 2 licenses.

Date: 30.08.2011, Norway.

http://www.webgrid.com
*/


#region Header

/*
Copyright ©  Olav Christian Botterli. 

Dual licensed under the MIT or GPL Version 2 licenses.

Date: 30.08.2011, Norway.

http://www.webgrid.com
*/
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion Header

namespace WebGrid.Util.Json
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Builds a string. Unlike StringBuilder this class lets you reuse it's internal buffer.
    /// </summary>
    internal class StringBuffer
    {
        #region Fields

        private static char[] _emptyBuffer = new char[0];

        private char[] _buffer;
        private int _position;

        #endregion Fields

        #region Constructors

        public StringBuffer()
        {
            _buffer = _emptyBuffer;
        }

        public StringBuffer(int initalSize)
        {
            _buffer = new char[initalSize];
        }

        #endregion Constructors

        #region Properties

        public int Position
        {
            get { return _position; }
              set { _position = value; }
        }

        #endregion Properties

        #region Methods

        public void Append(char value)
        {
            // test if the buffer array is large enough to take the value
              if (_position + 1 > _buffer.Length)
              {
            EnsureSize(1);
              }

              // set value and increment poisition
              _buffer[_position++] = value;
        }

        public void Clear()
        {
            _buffer = _emptyBuffer;
              _position = 0;
        }

        public override string ToString()
        {
            return new string(_buffer, 0, _position);
        }

        private void EnsureSize(int appendLength)
        {
            char[] newBuffer = new char[_position + appendLength * 2];

              Array.Copy(_buffer, newBuffer, _position);

              _buffer = newBuffer;
        }

        #endregion Methods
    }
}