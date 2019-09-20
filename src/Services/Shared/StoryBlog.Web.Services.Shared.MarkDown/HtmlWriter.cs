using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlWriter : IDisposable
    {
        private StreamWriter writer;
        private bool disposed;

        public HtmlWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        public void WriteTagStart(string tagName)
        {
            EnsureNotDisposed();

            writer.Write('<');
            writer.Write(tagName);
        }

        public void WriteTagClose(bool selfClosing)
        {
            EnsureNotDisposed();

            writer.Write(' ');

            if (selfClosing)
            {
                writer.Write('/');
            }

            writer.Write('>');
        }

        public void WriteTagEnd(string tagName)
        {
            EnsureNotDisposed();

            writer.Write('<');
            writer.Write('/');
            writer.Write(tagName);
            writer.Write('>');
        }

        public void WriteAttribute(string name, string value)
        {
            writer.Write(' ');
            writer.Write(name);
            writer.Write('=');
            writer.Write('\"');
            writer.Write(WebUtility.HtmlEncode(value));
            writer.Write('\"');
        }

        public void WriteContent(string content)
        {
            EnsureNotDisposed();

            writer.Write(WebUtility.HtmlEncode(content));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void WriteOpenTag(string tagName, bool closing)
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
                    //scopes = null;
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
        /*private sealed class ElementDisposable : IDisposable
        {
            private readonly HtmlWriter writer;
            private readonly string tag;

            public ElementDisposable(HtmlWriter writer, string tag)
            {
                this.writer = writer;
                this.tag = tag;
            }

            public void Dispose()
            {
                writer.WriteTag(tag, true);
            }
        }*/
    }
}