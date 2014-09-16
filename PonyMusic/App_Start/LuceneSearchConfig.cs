[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PonyMusic.App_Start.LuceneSearchConfig), "InitializeSearch")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PonyMusic.App_Start.LuceneSearchConfig),"FinalizeSearch")]



namespace PonyMusic.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;
    using PonyMusic.Models;

    public class LuceneSearchConfig
    {
        public static Directory directory;
        public static Analyzer analyzer;
        public static IndexWriter writer;

        public static void InitializeSearch()
        {
            string DirectoryPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes";
            directory = FSDirectory.Open(DirectoryPath);
            analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
            Task.Factory.StartNew(() => CreateIndex());
           
            
        }

        private static void CreateIndex()
        {
            Document doc;
            IEnumerable<Track> tracks =  Track.GetAllFromURLSync();
            foreach (var track in tracks)
            {
                doc = new Document();
                doc.Add(new Field("trackTitle", track.Title, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("artistName", track.Artist.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("trackMP3", track.Download.MP3, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("trackOGG", track.Download.Vorbis, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("trackLink", track.Link, Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("artistLink", track.Artist.Link, Field.Store.YES, Field.Index.NOT_ANALYZED));
                if(!String.IsNullOrEmpty(track.Download.Art)){
                    doc.Add(new Field("artLink", track.Download.Art, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }                
                writer.AddDocument(doc);
                doc = null;
            }
            writer.Optimize();
            writer.Commit();
            writer.Dispose();
            

        }

        public static void FinalizeSearch()
        {
            directory.Dispose();
        }

    }
}