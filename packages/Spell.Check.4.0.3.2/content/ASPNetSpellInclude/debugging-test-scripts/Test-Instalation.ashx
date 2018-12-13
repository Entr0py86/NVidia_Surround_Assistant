<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using ASPNetSpell;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class Handler : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {

        
        HttpResponse r = context.Response;
        HttpRequest req = context.Request;

        r.Write("ASP.Net Executing... <br/>");
        r.Write("ASPNetSpell - Testing DLL is loaded... <br/>");
        r.Flush();
        /// SETTINGS FILE///////////////
        ASPNetSpell.SpellChecker SpellEngine = new ASPNetSpell.SpellChecker();
        string localDictPath = req.MapPath("../dictionaries");
        SpellEngine.DictionaryPath = (localDictPath);

        r.Write("ASPNetSpell - Local Dictionary Path is: " + localDictPath + " <br/>");
        r.Flush();

        r.Write("ASPNetSpell - Setting Basic Settings <br/>");
        r.Flush();
        SpellEngine.IgnoreAllCaps = false;
        SpellEngine.IgnoreNumeric = false;
        SpellEngine.CaseSensitive = true;

        r.Write("ASPNetSpell - Loading Custom Dictionary:  "+localDictPath+"\\custom.txt <br/>");
        r.Flush();
        
        SpellEngine.LoadCustomDictionary("custom.txt");

        r.Write("ASPNetSpell - Loading Banned Words <br/>");
        r.Flush();
        SpellEngine.LoadCustomBannedWords("language-rules/banned-words.txt");

        r.Write("ASPNetSpell - Loading Enforced Corrections <br/>");
        r.Flush();
        
        SpellEngine.LoadEnforcedCorrections("language-rules/enforced-corrections.txt");
        r.Write("ASPNetSpell - Loading Common Typos <br/>");
        r.Flush();
        
        SpellEngine.LoadCommonTypos("language-rules/common-mistakes.txt");
        ////////////////////////////////
        

     

        




        

        string[] Langs =  SpellEngine.ListDictionaries();
        bool hasdicts = false;
        foreach (string dict in Langs)
        {
        hasdicts = true;
        r.Write("ASPNetSpell - Loading Dictionary : "+dict+" <br/>");
        r.Flush();
        
            SpellEngine.LoadDictionary(dict.Trim());
        }
        
        if(!hasdicts){r.Write("NO DICTIONARIES LOADED - CHECK FILE PERMISSIONS <br/>");}


       string[] testwords = "good,bahd,woorld,jour".Split(','); 
        
        foreach (string testword in testwords){
                r.Write("ASPNetSpell - Spell Checking '"+testword+"' - Result: "+(SpellEngine.SpellCheckWord(testword)?"TRUE":"FALSE")+" <br/>");
                 r.Flush();
        
        
                string strSuggestions = String.Join( ",",SpellEngine.Suggestions(testword));
           
                 r.Write("ASPNetSpell - Suggestions for  '"+testword+"' - Result: "+ strSuggestions +" <br/>");
                  r.Flush();
        
        }
        
         r.Write("<hr/>ASPNetSpell - Test Complete <br/>");
         r.Flush();
        
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}          