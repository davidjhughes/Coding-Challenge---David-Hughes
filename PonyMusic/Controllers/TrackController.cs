using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using PonyMusic.Models;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Diagnostics;


namespace PonyMusic.Controllers
{
    public class TrackController : Controller
    {
        public ActionResult Index(string query)
        {
            TrackSearchResult tsResult = new TrackSearchResult();
            return View(tsResult.GetTrackSearchResults(query, 500));
        }


        public JsonResult GetTrackTitles(string query)
        {

            TrackSearchResult tsResult = new TrackSearchResult();
            List<TrackSearchResult> suggestions = tsResult.GetTrackSearchResults(query, 20);
            string[] titles = new string[suggestions.Count];
            for (var i = 0; i < suggestions.Count; i++ )
            {
                titles[i] = suggestions.ElementAt(i).TrackTitle;
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        
    }
}