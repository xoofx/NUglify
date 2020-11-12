// IVisitor.cs
//
// Copyright 2011 Microsoft Corporation
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

using NUglify.JavaScript.Syntax;

namespace NUglify.JavaScript.Visitors
{
    public interface IVisitor
    {
        void Visit(ArrayLiteral node);
        void Visit(AspNetBlockNode node);
        void Visit(AstNodeList node);
        void Visit(BinaryExpression node);
        void Visit(BindingIdentifier node);
        void Visit(BlockStatement node);
        void Visit(BreakStatement node);
        void Visit(CallExpression node);
        void Visit(ClassNode node);
        void Visit(ComprehensionNode node);
        void Visit(ComprehensionForClause node);
        void Visit(ComprehensionIfClause node);
        void Visit(ConditionalCompilationComment node);
        void Visit(ConditionalCompilationElse node);
        void Visit(ConditionalCompilationElseIf node);
        void Visit(ConditionalCompilationEnd node);
        void Visit(ConditionalCompilationIf node);
        void Visit(ConditionalCompilationOn node);
        void Visit(ConditionalCompilationSet node);
        void Visit(Conditional node);
        void Visit(ConstantWrapper node);
        void Visit(ConstantWrapperPP node);
        void Visit(ConstStatement node);
        void Visit(ContinueStatement node);
        void Visit(ComputedPropertyField node);
        void Visit(CustomNode node);
        void Visit(DebuggerNode node);
        void Visit(DirectivePrologue node);
        void Visit(DoWhileStatement node);
        void Visit(EmptyStatement node);
        void Visit(ExportStatement node);
        void Visit(ForInStatement node);
        void Visit(ForStatement node);
        void Visit(FunctionObject node);
        void Visit(GetterSetter node);
        void Visit(GroupingOperator node);
        void Visit(IfStatement node);
        void Visit(StandardComment node);
        void Visit(ImportantComment node);
        void Visit(ImportExportSpecifier node);
        void Visit(ImportStatement node);
        void Visit(InitializerNode node);
        void Visit(LabeledStatement node);
        void Visit(LexicalDeclaration node);
        void Visit(LookupExpression node);
        void Visit(MemberExpression node);
        void Visit(ModuleDeclaration node);
        void Visit(ObjectLiteral node);
        void Visit(ObjectLiteralField node);
        void Visit(ObjectLiteralProperty node);
        void Visit(ParameterDeclaration node);
        void Visit(RegExpLiteral node);
        void Visit(ReturnStatement node);
        void Visit(SwitchStatement node);
        void Visit(SwitchCase node);
        void Visit(TemplateLiteral node);
        void Visit(TemplateLiteralExpression node);
        void Visit(ThisLiteral node);
        void Visit(ThrowStatement node);
        void Visit(TryStatement node);
        void Visit(VarDeclaration node);
        void Visit(VariableDeclaration node);
        void Visit(UnaryExpression node);
        void Visit(WhileStatement node);
        void Visit(WithStatement node);
    }
}
