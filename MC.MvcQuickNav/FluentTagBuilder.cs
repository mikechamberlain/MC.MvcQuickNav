using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MC.MvcQuickNav
{
    /// <summary>
    /// Wrapper for <see cref="TagBuilder"/> that makes
    /// it a fluent API.
    /// </summary>
    internal class FluentTagBuilder
    {
        private readonly TagBuilder _builder;
        private bool _conditionState = true;

        public FluentTagBuilder(string tagName)
        {
            _builder = new TagBuilder(tagName);
        }

        public FluentTagBuilder MergeAttribute(string key, string value)
        {
            return Operate(() => _builder.MergeAttribute(key, value));
        }

        public FluentTagBuilder AddCssClass(string cssClass)
        {
            return Operate(() => _builder.AddCssClass(cssClass));
        }

        public FluentTagBuilder SetInnerText(string text)
        {
            return Operate(() => _builder.SetInnerText(text));
        }

        public FluentTagBuilder SetInnerHtml(string html)
        {
            return Operate(() => _builder.InnerHtml = html);
        }

        public FluentTagBuilder AddInnerTag(FluentTagBuilder tag)
        {
            return Operate(() => _builder.InnerHtml += tag.ToString());
        }

        public FluentTagBuilder If(bool condition)
        {
            _conditionState = condition;
            return this;
        }

        private FluentTagBuilder Operate(Action operation)
        {
            if (_conditionState)
                operation();
            _conditionState = true;
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
    