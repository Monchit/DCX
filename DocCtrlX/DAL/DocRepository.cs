using System;
using System.Collections.Generic;
using System.Linq;
using DocCtrlX.Models;
using System.Data;

namespace DocCtrlX.DAL
{
    public class DocRepository : IDocRepository, IDisposable
    {
        private DocCtrlXEntities context;

        public DocRepository(DocCtrlXEntities context)
        {
            this.context = context;
        }

        public IEnumerable<td_doc> GetDocs()
        {
            return context.td_doc.ToList();
        }

        public td_doc GetDocByKey(string doc_no)
        {
            return context.td_doc.Find(doc_no);
        }

        public void InsertDoc(td_doc doc)
        {
            context.td_doc.Add(doc);
        }

        public void DeleteDoc(string doc_no)
        {
            td_doc doc = context.td_doc.Find(doc_no);
            context.td_doc.Remove(doc);
        }

        public void UpdateDoc(td_doc doc)
        {
            context.Entry(doc).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}