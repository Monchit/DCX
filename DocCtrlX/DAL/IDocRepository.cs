using System;
using System.Collections.Generic;
using DocCtrlX.Models;

namespace DocCtrlX.DAL
{
    public interface IDocRepository : IDisposable
    {
        IEnumerable<td_doc> GetDocs();
        td_doc GetDocByKey(string doc_no);
        void InsertDoc(td_doc doc);
        void DeleteDoc(string doc_no);
        void UpdateDoc(td_doc doc);
        void Save();
    }
}