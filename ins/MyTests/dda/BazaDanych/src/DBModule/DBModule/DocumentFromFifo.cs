using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DBModule
{
    public class PackedDocument
    {
        public ObjectId id;
        public Document document;
    }

    public class DocumentFromFifo:IDataBaseGetDocument
    {
        MongoCollection<PackedDocument> FIFO;

        public PackedDocument packDocument(Document newDocument)
        {
            PackedDocument packedDocument = new PackedDocument { document = newDocument };
            return packedDocument;
        }

        public Document unpackDocument(PackedDocument packedDocument)
        {
            Document unpackedDocument = packedDocument.document;
            return unpackedDocument;
        }

        public void AddDocument(Document document)
        {
        
            PackedDocument packed = packDocument(document);
            FIFO.Insert(packed);
        }

        public Document GetDocument()
        {
            PackedDocument packed = FIFO.FindOne();
            if (packed != null)
            {
                var _id1 = packed.id;
                var query = Query.EQ("_id", _id1);
                FIFO.Remove(query);
                Document unpacked = unpackDocument(packed);
                return unpacked;
            }
            else
            {
                return null;
            }
        }
        public DocumentFromFifo()
        {
            FIFO = DBConnection.db.GetCollection<PackedDocument>("FIFO");
        }

    }
}
