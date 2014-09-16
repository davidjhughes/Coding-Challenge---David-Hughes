using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

using PonyMusic.Helpers;





namespace PonyMusic.Models
{
    public class TrackSearchResult
    {
        public string TrackTitle { get; set; }
        public string ArtistName { get; set; }
        public string MP3 { get; set; }
        public string OGG { get; set; }
        public string TrackLink { get; set; }
        public string ArtistLink { get; set; }
        public string ArtLink { get; set; }

        public List<TrackSearchResult> GetTrackSearchResults(string query, int numberOfResults, Boolean titlesort = false)
        {
            List<TrackSearchResult> searchResults = new List<TrackSearchResult>();
            if (string.IsNullOrEmpty(query))
            {
                query = "a* b* c* d* e* f* g* h* i* j* k* l* m* n* o* p* q* r* s* t* u* v* w* x* y* z*";
                titlesort = true;
            }
            else
            {
                query = Helper.CleanInput(query).Trim() + "*";
            }
            string indexDirectory = HttpContext.Current.Server.MapPath("~/App_Data/LuceneIndexes");
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(indexDirectory));
            var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "trackTitle", analyzer);
            Query searchQuery = parser.Parse(query);
            TopDocs hits = searcher.Search(searchQuery, numberOfResults);
            int results = hits.TotalHits;
            TrackSearchResult searchResult = null;
            for (int i = 0; i < hits.ScoreDocs.Count(); i++)
            {
                
                Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                searchResult = new TrackSearchResult();
                searchResult.TrackTitle = doc.Get("trackTitle");
                searchResult.ArtistName = doc.Get("artistName");
                searchResult.MP3 = doc.Get("trackMP3");
                searchResult.OGG = doc.Get("trackOGG");
                searchResult.TrackLink = doc.Get("trackLink");
                searchResult.ArtistLink = doc.Get("artistLink");
                searchResult.ArtistLink = doc.Get("artLink");
                if(searchResults.FindAll(x=> x.MP3 == searchResult.MP3).Count == 0){
                    searchResults.Add(searchResult);
                }
                
                searchResult = null;
            }

            

            if (titlesort)
            {
                searchResults.Sort(delegate(TrackSearchResult x, TrackSearchResult y)
                {
                    return x.TrackTitle.CompareTo(y.TrackTitle);
                });
            }
            

            return searchResults;
        }

       

    }

   
}