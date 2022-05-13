using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DocumentStorage
    {
        public List<Document> GetFullList()
        {
            using var context = new ApplicationContext();
            return context.Documents
            .ToList();
        }
        public List<Document> GetFiltredList(string str)
        {
            using var context = new ApplicationContext();
            return context.Documents.Where(r => r.Id.Contains(str))
            .ToList();
        }
        public void Insert(Document model)
        {
            using var context = new ApplicationContext();
            context.Documents.Add(model);
            context.SaveChanges();
        }
        public Document GetElement(string id)
        {
            using var context = new ApplicationContext();
            var element = context.Documents
            .FirstOrDefault(rec => rec.Id == id);
            return element;
        }
        public void Update(Document model)
        {
            using var context = new ApplicationContext();
            var element = context.Documents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }

            element.DateUpdate = model.DateUpdate;
            element.DateDocument = model.DateDocument;
            element.Id = model.Id;
            element.RolePersonChange = model.RolePersonChange;
            element.Barcode = model.Barcode;
            element.DeliveryNumber = model.DeliveryNumber;
            element.Workshop = model.Workshop;
            element.DocumentType = model.DocumentType;
            element.Note = model.Note;
            element.Stage = model.Stage;
            context.SaveChanges();
        }
        public void Delete(string id)
        {
            using var context = new ApplicationContext();
            Document element = context.Documents.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Documents.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        public void SetStateReturn(string id)
        {
            using var context = new ApplicationContext();
            Document element = context.Documents.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                element.Stage = "Возвращен на уточнение";
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        public void SetStateAccept(string id)
        {
            using var context = new ApplicationContext();
            Document element = context.Documents.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                element.Stage = "Принят к учету";
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
