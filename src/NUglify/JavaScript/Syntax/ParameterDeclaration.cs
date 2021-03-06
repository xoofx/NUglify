// parameterdeclaration.cs
//
// Copyright 2010 Microsoft Corporation
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

using System.Collections.Generic;
using NUglify.Helpers;
using NUglify.JavaScript.Visitors;

namespace NUglify.JavaScript.Syntax
{
    public sealed class ParameterDeclaration : AstNode
    {
        AstNode binding;
        AstNode initializer;

        /// <summary>
        /// Gets or sets parameter order position, zero-based
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets whether this parameter is prefixed with the rest token (...)
        /// </summary>
        public bool HasRest { get; set; }

        /// <summary>
        /// Gets or sets the source context for the rest token (if any)
        /// </summary>
        public SourceContext RestContext { get; set; }

        /// <summary>
        /// Gets or sets the binding node
        /// </summary>
        public AstNode Binding 
        {
            get => binding;
            set => ReplaceNode(ref binding, value);
        }

        /// <summary>
        /// Gets or sets the context for the optional default-value assignment token
        /// </summary>
        public SourceContext AssignContext { get; set; }

        /// <summary>
        /// Gets or sets the optional default value for the parameter
        /// </summary>
        public AstNode Initializer 
        {
            get => initializer;
            set => ReplaceNode(ref initializer, value);
        }

        public override IEnumerable<AstNode> Children => EnumerateNonNullNodes(Binding, Initializer);

        public bool IsReferenced
        {
            get
            {
                // the entire parameter is referenced if ANY of the binding declarations 
                // within it have a reference
                foreach(var nameDecl in BindingsVisitor.Bindings(this))
                {
                    // if there is no variable field (although there SHOULD be), then let's
                    // just assume it's referenced.
                    if (nameDecl.VariableField == null || nameDecl.VariableField.IsReferenced)
                        return true;
                }

                // if we get here, none are referenced, so this parameter is not referenced
                return false;
            }
        }

        public ParameterDeclaration(SourceContext context)
            : base(context)
        {
        }

        public override void Accept(IVisitor visitor)
        {
	        visitor?.Visit(this);
        }

        internal override string GetFunctionGuess(AstNode target)
        {
            return Binding.IfNotNull(b => b.GetFunctionGuess(target));
        }

        public override bool ReplaceChild(AstNode oldNode, AstNode newNode)
        {
            if (Binding == oldNode)
            {
                Binding = newNode;
                return true;
            }

            if (Initializer == oldNode)
            {
	            Initializer = newNode;
	            return true;
            }

            return false;
        }
    }
}
