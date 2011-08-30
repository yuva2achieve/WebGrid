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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    ///<summary>
    ///</summary>
    public class JavaScriptParameters : ReadOnlyCollection<object>
    {
        #region Fields

        ///<summary>
        ///</summary>
        public static readonly JavaScriptParameters Empty = new JavaScriptParameters(new List<object>());

        #endregion Fields

        #region Constructors

        ///<summary>
        ///</summary>
        ///<param name="list"></param>
        public JavaScriptParameters(IList<object> list)
            : base(list)
        {
        }

        #endregion Constructors
    }
}