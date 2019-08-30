using StoryBlog.Web.Services.Blog.Interop.Markups.Nodes;
using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;
using StoryBlog.Web.Services.Shared.Common;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Interop.Markups.Composing
{
    public class BulletingBoardDocumentVisitor : IVisitor<BulletingBoardDocument>
    {
        public virtual void Visit(BulletingBoardDocument obj)
        {
            RenderDocument(obj);
        }

        protected virtual void RenderStrong(BulletingBoardStrong strong)
        {
            RenderBlockElements(strong.Elements);
        }

        protected virtual void RenderEmphasized(BulletingBoardEmphasized em)
        {
            RenderBlockElements(em.Elements);
        }

        protected virtual void RenderCite(BulletingBoardCite cite)
        {
            RenderBlockElements(cite.Elements);
        }

        protected virtual void RenderAbbr(BulletingBoardAbbr abbr)
        {
            RenderBlockElements(abbr.Elements);
        }

        protected virtual void RenderTerm(BulletingBoardTerm term)
        {
            RenderBlockElements(term.Elements);
        }

        protected virtual void RenderItalic(BulletingBoardItalic italic)
        {
            RenderBlockElements(italic.Elements);
        }

        protected virtual void RenderUnderline(BulletingBoardUnderline underline)
        {
            RenderBlockElements(underline.Elements);
        }

        protected virtual void RenderHyperlink(BulletingBoardHyperlink link)
        {
            RenderBlockElements(link.Elements);
        }

        protected virtual void RenderText(BulletingBoardText text)
        {
        }

        protected virtual void RenderDocument(BulletingBoardDocument document)
        {
            RenderBlockElements(document.Elements);
        }

        protected void RenderElement(BulletingBoardElement element)
        {
            if (element is BulletingBoardBlock block)
            {
                RenderBlock(block);
                return;
            }

            RenderInline((BulletingBoardInline)element);
        }

        private void RenderBlock(BulletingBoardBlock block)
        {
            switch (block.BlockType)
            {
                case BulletingBoardBlockType.Em:
                {
                    RenderEmphasized((BulletingBoardEmphasized) block);
                    break;
                }

                case BulletingBoardBlockType.Cite:
                {
                    RenderCite((BulletingBoardCite) block);
                    break;
                }

                case BulletingBoardBlockType.Abbr:
                {
                    RenderAbbr((BulletingBoardAbbr) block);
                    break;
                }

                case BulletingBoardBlockType.Dfn:
                {
                    RenderTerm((BulletingBoardTerm) block);
                    break;
                }


                case BulletingBoardBlockType.Italic:
                {
                    RenderItalic((BulletingBoardItalic) block);
                    break;
                }

                case BulletingBoardBlockType.Strong:
                {
                    RenderStrong((BulletingBoardStrong) block);
                    break;
                }

                case BulletingBoardBlockType.Underline:
                {
                    RenderUnderline((BulletingBoardUnderline) block);
                    break;
                }

                case BulletingBoardBlockType.Hyperlink:
                {
                    RenderHyperlink((BulletingBoardHyperlink) block);
                    break;
                }

                default:
                {
                    DoRenderUnknownBlock(block);
                    break;
                }
            }
        }

        private void RenderInline(BulletingBoardInline inline)
        {
            switch (inline.InlineType)
            {
                case BulletingBoardInlineType.Text:
                {
                    RenderText((BulletingBoardText) inline);
                    break;
                }
            }
        }

        private void RenderBlockElements(IEnumerable<BulletingBoardElement> elements)
        {
            foreach (var element in elements)
            {
                RenderElement(element);
            }
        }

        private void DoRenderUnknownBlock(BulletingBoardBlock block)
        {
            throw new System.NotImplementedException();
        }
    }
}