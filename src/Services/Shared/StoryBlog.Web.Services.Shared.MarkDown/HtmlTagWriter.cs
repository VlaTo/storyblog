using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public sealed class HtmlTag
    {
        public string TagName
        {
            get;
        }

        public HtmlTag()
        {
        }

        public void WriteOpen()
        {

        }

        public void WriteClose()
        {

        }
    }

    public class HtmlTagWriter : IDisposable
    {
        private StreamWriter writer;
        private Stack<ElementDisposable> scopes;
        private bool disposed;

        public HtmlTagWriter(StreamWriter writer)
        {
            this.writer = writer;
            scopes = new Stack<ElementDisposable>();
        }

        public HtmlTag OpenElement(string tag)
        {
            EnsureNotDisposed();

            var disposable = new ElementDisposable(this, tag);

            scopes.Push(disposable);
            WriteTag(tag, false);

            return null;
        }

        public void AddContent(string content)
        {
            EnsureNotDisposed();

            if (0 == scopes.Count)
            {
                throw new Exception();
            }

            var text = WebUtility.HtmlEncode(content);

            writer.Write(text);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void WriteTag(string tagName, bool closing)
        {
            writer.Write('<');

            if (closing)
            {
                writer.Write('/');
            }

            writer.Write(tagName);
            writer.Write('>');
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    writer.Dispose();
                    writer = null;
                    scopes = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class ElementDisposable : IDisposable
        {
            private readonly HtmlTagWriter writer;
            private readonly string tag;

            public ElementDisposable(HtmlTagWriter writer, string tag)
            {
                this.writer = writer;
                this.tag = tag;
            }

            public void Dispose()
            {
                writer.WriteTag(tag, true);
            }
        }
    }
}