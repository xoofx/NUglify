// JsonOutputVisitor.cs
//
// Copyright 2012 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Globalization;
using System.IO;
using NUglify.JavaScript.Syntax;

namespace NUglify.JavaScript.Visitors
{

    /// <summary>
    /// output JSON-compatible code
    /// </summary>
    public class JsonOutputVisitor : IVisitor
    {
	    readonly TextWriter writer;
	    readonly CodeSettings settings;
	    int indentLevel;

        public bool IsValid { get; private set; }

        JsonOutputVisitor(TextWriter writer, CodeSettings settings)
        {
            this.writer = writer;
            this.settings = settings;
            this.IsValid = true;
            this.indentLevel = 0;
        }

        public static bool Apply(TextWriter writer, AstNode node, CodeSettings settings)
        {
	        if (node == null)
		        return false;

	        var visitor = new JsonOutputVisitor(writer, settings);
            node.Accept(visitor);
            return visitor.IsValid;
        }

        public void Visit(ArrayLiteral node)
        {
            if (node != null)
            {
                // if this is multi-line output, we're going to want to run some checks first
                // to see if we want to put the array all on one line or put elements on separate lines.
                var multiLine = false;
                if (settings.OutputMode == OutputMode.MultipleLines)
                {
                    if (node.Elements.Count > 5 || NotJustPrimitives(node.Elements))
                    {
                        multiLine = true;
                    }
                }

                writer.Write('[');
                if (node.Elements != null)
                {
                    if (multiLine)
                    {
                        // multiline -- let's pretty it up a bit
                        Indent();
                        try
                        {
                            var first = true;
                            foreach (var element in node.Elements)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    writer.Write(',');
                                }

                                NewLine();
                                element.Accept(this);
                            }
                        }
                        finally
                        {
                            Unindent();
                        }

                        NewLine();
                    }
                    else
                    {
                        // not multiline, so just run through all the items
                        node.Elements.Accept(this);
                    }
                }

                writer.Write(']');
            }
        }

        public void Visit(ComputedPropertyField node)
        {
            if (node != null && node.ArrayNode != null)
            {
                // if this is multi-line output, we're going to want to run some checks first
                // to see if we want to put the array all on one line or put elements on separate lines.
                var multiLine = false;
                if (settings.OutputMode == OutputMode.MultipleLines)
                {
                    if (node.ArrayNode.Elements.Count > 5 || NotJustPrimitives(node.ArrayNode.Elements))
                    {
                        multiLine = true;
                    }
                }

                writer.Write('[');
                if (node.ArrayNode.Elements != null)
                {
                    if (multiLine)
                    {
                        // multiline -- let's pretty it up a bit
                        Indent();
                        try
                        {
                            var first = true;
                            foreach (var element in node.ArrayNode.Elements)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    writer.Write(',');
                                }

                                NewLine();
                                element.Accept(this);
                            }
                        }
                        finally
                        {
                            Unindent();
                        }

                        NewLine();
                    }
                    else
                    {
                        // not multiline, so just run through all the items
                        node.ArrayNode.Elements.Accept(this);
                    }
                }

                writer.Write(']');
            }
        }

        public void Visit(AstNodeList node)
        {
            if (node != null)
            {
                for (var ndx = 0; ndx < node.Count; ++ndx)
                {
                    if (ndx > 0)
                    {
                        writer.Write(',');
                        if (settings.OutputMode == OutputMode.MultipleLines)
                        {
                            writer.Write(' ');
                        }
                    }

                    if (node[ndx] != null)
                    {
                        node[ndx].Accept(this);
                    }
                }
            }
        }

        public void Visit(BlockStatement node)
        {
            if (node != null && node.Count > 0)
            {
                // there should only be one "statement"
                node[0].Accept(this);
            }
        }

        public void Visit(ConstantWrapper node)
        {
            if (node != null)
            {
                // allow string, number, true, false, and null.
                switch (node.PrimitiveType)
                {
                    case PrimitiveType.Boolean:
                        writer.Write((bool)node.Value ? "true" : "false");
                        break;

                    case PrimitiveType.Null:
                        writer.Write("null");
                        break;

                    case PrimitiveType.Number:
                        OutputNumber((double)node.Value, node.Context);
                        break;

                    case PrimitiveType.String:
                    case PrimitiveType.Other:
                        // string -- or treat it like a string
                        OutputString(node.Value.ToString());
                        break;
                }
            }
        }

        public void Visit(CustomNode node)
        {
            if (node != null)
            {
                // whatever people plug in. Hopefully it's valid JSON.
                OutputString(node.ToCode());
            }
        }

        public void Visit(UnaryExpression node)
        {
            if (node != null)
            {
                // only a negation is allowed -- and even then, I'm not sure
                // if it has already been integrated into the numeric value yet.
                if (node.OperatorToken == JSToken.Minus)
                {
                    writer.Write('-');
                    if (node.Operand != null)
                    {
                        node.Operand.Accept(this);
                    }
                }
                else
                {
                    // invalid! ignore
                    IsValid = false;
                }
            }
        }

        public void Visit(ObjectLiteral node)
        {
            if (node != null)
            {
                writer.Write('{');
                if (node.Properties != null)
                {
                    // if this is multi-line output, we're going to want to run some checks first
                    // to see if we want to put the array all on one line or put elements on separate lines.
                    var multiLine = false;
                    if (settings.OutputMode == OutputMode.MultipleLines)
                    {
                        if (node.Properties.Count > 5 || NotJustPrimitives(node.Properties))
                        {
                            multiLine = true;
                        }
                    }

                    if (multiLine)
                    {
                        // multiline -- let's pretty it up a bit
                        Indent();
                        try
                        {
                            var first = true;
                            foreach (var property in node.Properties)
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    writer.Write(',');
                                }

                                NewLine();
                                property.Accept(this);
                            }
                        }
                        finally
                        {
                            Unindent();
                        }

                        NewLine();
                    }
                    else
                    {
                        node.Properties.Accept(this);
                    }
                }

                writer.Write('}');
            }
        }

        public void Visit(ObjectLiteralField node)
        {
            if (node != null)
            {
                if (node.PrimitiveType == PrimitiveType.String)
                {
                    // must be double-quoted string with a limited number of escapes
                    OutputString(node.Value.ToString());
                }
                else
                {
                    // really the property names can only be strings, so this 
                    // branch means the input was invalid.
                    writer.Write('"');
                    Visit(node as ConstantWrapper);
                    writer.Write('"');
                }
            }
        }

        public void Visit(ObjectLiteralProperty node)
        {
            if (node != null)
            {
                if (node.Name != null)
                {
                    node.Name.Accept(this);
                }

                writer.Write(':');

                if (node.Value != null)
                {
                    node.Value.Accept(this);
                }
            }
        }

        
        public void Visit(AspNetBlockNode node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(BinaryExpression node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(BindingIdentifier node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(BreakStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ClassNode node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ComprehensionNode node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ComprehensionForClause node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ComprehensionIfClause node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(CallExpression node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationComment node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationElse node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationElseIf node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationEnd node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationIf node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationOn node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConditionalCompilationSet node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(Conditional node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConstantWrapperPP node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ConstStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ContinueStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(DebuggerNode node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(DirectivePrologue node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(DoWhileStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(EmptyStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ExportStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ForInStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ForStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(FunctionObject node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(GetterSetter node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(GroupingOperator node)
        {
            // not TECHNICALLY valid! set the invalid flag, but
            // still recurse the operand, just in case
            IsValid = false;
            if (node != null && node.Operand != null)
            {
                node.Operand.Accept(this);
            }
        }

        public void Visit(IfStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(Syntax.Comment node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ImportExportSpecifier node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ImportStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(InitializerNode node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(LabeledStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(LexicalDeclaration node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(LookupExpression node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(MemberExpression node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ModuleDeclaration node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ParameterDeclaration node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(RegExpLiteral node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ReturnStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(SwitchStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(SwitchCase node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(TemplateLiteral node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(TemplateLiteralExpression node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ThisLiteral node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(ThrowStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(TryStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(VarDeclaration node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(VariableDeclaration node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(WhileStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        public void Visit(WithStatement node)
        {
            // invalid! ignore
            IsValid = false;
        }

        void OutputString(string text)
        {
            // must be double-quote delimited
            writer.Write('"');
            for (var ndx = 0; ndx < text.Length; ++ndx)
            {
                var ch = text[ndx];
                switch (ch)
                {
                    case '\"':
                        writer.Write("\\\"");
                        break;

                    case '\b':
                        writer.Write("\\b");
                        break;

                    case '\f':
                        writer.Write("\\f");
                        break;

                    case '\n':
                        writer.Write("\\n");
                        break;

                    case '\r':
                        writer.Write("\\r");
                        break;

                    case '\t':
                        writer.Write("\\t");
                        break;

                    case '\\':
                        writer.Write("\\\\");
                        break;

                    default:
                        if (ch < ' ')
                        {
                            // other control characters must be escaped as \uXXXX
                            writer.Write("\\u{0:x4}", (int)ch);
                        }
                        else
                        {
                            // just append it. The output encoding will take care of the rest
                            writer.Write(ch);
                        }
                        break;
                }
            }

            writer.Write('"');
        }

        public void OutputNumber(double numericValue, SourceContext originalContext)
        {
            // numerics are doubles in JavaScript, so force it now as a shortcut
            if (double.IsNaN(numericValue) || double.IsInfinity(numericValue))
            {
                // weird number -- just return the original source code as-is. 
                if (originalContext != null && !string.IsNullOrEmpty(originalContext.Code)
                    && !originalContext.Document.IsGenerated)
                {
                    writer.Write(originalContext.Code);
                    return;
                }

                // Hmmm... don't have an original source. 
                // Must be generated. Just generate the proper JS literal.
                //
                // DANGER! If we just output NaN and Infinity and -Infinity blindly, that assumes
                // that there aren't any local variables in this scope chain with that
                // name, and we're pulling the GLOBAL properties. Might want to use properties
                // on the Number object -- which, of course, assumes that Number doesn't
                // resolve to a local variable...
                string objectName = double.IsNaN(numericValue) ? "NaN" : "Infinity";

                // we're good to go -- just return the name because it will resolve to the
                // global properties (make a special case for negative infinity)
                writer.Write(double.IsNegativeInfinity(numericValue) ? "-Infinity" : objectName);
            }
            else if (numericValue == 0)
            {
                // special case zero because we don't need to go through all those
                // gyrations to get a "0" -- and because negative zero is different
                // than a positive zero
                writer.Write(1 / numericValue < 0 ? "-0" : "0");
            }
            else
            {
                // normal string representations
                writer.Write(GetSmallestRep(numericValue.ToString("R", CultureInfo.InvariantCulture)));
            }
        }

        static string GetSmallestRep(string number)
        {
            var match = CommonData.DecimalFormat.Match(number);
            if (match.Success)
            {
                string mantissa = match.Result("${man}");
                if (string.IsNullOrEmpty(match.Result("${exp}")))
                {
                    if (string.IsNullOrEmpty(mantissa))
                    {
                        // no decimal portion
                        if (string.IsNullOrEmpty(match.Result("${sig}")))
                        {
                            // no non-zero digits in the magnitude either -- must be a zero
                            number = match.Result("${neg}") + "0";
                        }
                        else
                        {
                            // see if there are trailing zeros
                            // that we can use e-notation to make smaller
                            int numZeros = match.Result("${zer}").Length;
                            if (numZeros > 2)
                            {
                                number = match.Result("${neg}") + match.Result("${sig}")
                                    + 'e' + numZeros.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                    }
                    else
                    {
                        // there is a decimal portion. Put it back together
                        // with the bare-minimum stuff -- no plus-sign, no leading magnitude zeros,
                        // no trailing mantissa zeros. A zero magnitude won't show up, either.
                        number = match.Result("${neg}") + match.Result("${mag}") + '.' + mantissa;
                    }
                }
                else if (string.IsNullOrEmpty(mantissa))
                {
                    // there is an exponent, but no significant mantissa
                    number = match.Result("${neg}") + match.Result("${mag}")
                        + "e" + match.Result("${eng}") + match.Result("${pow}");
                }
                else
                {
                    // there is an exponent and a significant mantissa
                    // we want to see if we can eliminate it and save some bytes

                    // get the integer value of the exponent
                    int exponent;
                    if (int.TryParse(match.Result("${eng}") + match.Result("${pow}"), NumberStyles.Integer, CultureInfo.InvariantCulture, out exponent))
                    {
                        // slap the mantissa directly to the magnitude without a decimal point.
                        // we'll subtract the number of characters we just added to the magnitude from
                        // the exponent
                        number = match.Result("${neg}") + match.Result("${mag}") + mantissa
                            + 'e' + (exponent - mantissa.Length).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        // should n't get here, but it we do, go with what we have
                        number = match.Result("${neg}") + match.Result("${mag}") + '.' + mantissa
                            + 'e' + match.Result("${eng}") + match.Result("${pow}");
                    }
                }
            }

            return number;
        }


        static bool NotJustPrimitives(AstNodeList nodeList)
        {
            // if any node in the list isn't a constant wrapper (boolean, number, string)
            // or a unary (presumably a negative number), then we've got something other than
            // a primitive in the list (array or object)
            foreach (var child in nodeList)
            {
                if (!(child is ConstantWrapper) && !(child is UnaryExpression))
                {
                    return true;
                }
            }

            // if we get here, then everything is a primitive
            return false;
        }

        /// <summary>
        ///  output a new line and setup the proper indent level
        /// </summary>
        void NewLine()
        {
            writer.WriteLine();
            for(var i = 0; i < this.indentLevel; i++)
				writer.Write(settings.Indent);
        }

        void Unindent()
        {
	        if (this.indentLevel > 0)
		        this.indentLevel--;
        }

        void Indent()
        {
	        this.indentLevel++;
        }
    }
}
