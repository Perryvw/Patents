using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Patents
{
    class PatentFamily
    {
        public String Title;
        public List<String> Patents;
        public String Inventor;
        public String DWAN;
        public String Abstract;
        public List<String> Applicants;
        public List<String> Citations;
        public List<String> Companies;
        public List<String> Countries;
        public List<String> IPCCodes;
        public int Year;
        public int Cited = 0;

        public PatentFamily(String line)
        {
            Patents = new List<string>();
            Applicants = new List<string>();
            Citations = new List<string>();
            Companies = new List<string>();
            Countries = new List<string>();
            IPCCodes = new List<string>();

            //Split the string on tabs
            String[] splitString = line.Split('\t');

            //Title
            Title = splitString[1];

            //Abstract
            Abstract = splitString[5].Trim();

            //Patent numbers
            Patents.AddRange(splitString[0].Replace(" ","").Split(';'));

            //Get the countries the indivisual patents were requested in
            //(Removes duplicates)
            for (int i = 0; i < Patents.Count; i++)
            {
                String country = Patents[i].Substring(0, 2);
                if (!Countries.Contains(country))
                {
                    Countries.Add(country);
                }
            }

            //Get the IPC codes
            String[] ipcCodes = splitString[10].Replace("; ",";").Split(';');
            for (int i = 0; i < ipcCodes.Length; i++)
            {
                IPCCodes.Add(ipcCodes[i].Trim());
            }

            //Derwent ascension number
            DWAN = splitString[4];

            //Get the inventor
            Inventor = splitString[2];

            //Get asignees
            Applicants.AddRange(splitString[3].Replace(";  ", ";").Replace("; ", ";").Split(';'));

            //Patent request year
            Year = int.Parse(splitString[12].Split(' ').Last());

            //Citations
            String[] splitCitations = splitString[17].Replace(" ; ", ";").Replace("; ", ";").Split(';');
            foreach (String citation in splitCitations)
            {
                String firstWord = citation.Split(' ')[0];
                if (firstWord.Contains('-'))
                {
                    Citations.Add(firstWord);
                }
            }

            //Get companies (same as asignees but filters out invidisual asignees)
            String[] splitCompanies = splitString[3].Split(';');
            foreach (String company in splitCompanies)
            {
                String name = company.Substring(company.IndexOf('(') + 1).Split(')')[0];
                if (!Companies.Contains(name) && !name.Contains("-Individual"))
                {
                    Companies.Add(name);
                }
            }
        }
    }
}
