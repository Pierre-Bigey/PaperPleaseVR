using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Entrants
{
    

    public class Rule
    {
        public class RuleSubject
        {
            public readonly bool country; //True if entrant's country, false if entrant's type
            public readonly List<Country> countriesSubjects;
            public readonly List<EntrantType> entrantTypeSubject;

            public RuleSubject( List<Country> _countriesSubjects = null,List<EntrantType> _entrantTypeSubject= null, bool _country = true)
            {
                country = _country;
                countriesSubjects = _countriesSubjects;
                entrantTypeSubject = _entrantTypeSubject;
            }

        }
        
        #region rulesEnums
        public enum RuleSubjectShortcut{
            ALL,
            FOREIGNER,
            CITIZEN,
            SPEC_CITIZEN,
            SPEC_TYPE
        }
        public enum RuleCondition
        {
            HAVE_DOCUMENT,
            HAVE_CONTRABAND,
            ALWAYS
        }
        public enum RuleAction
        {
            ENTER,
            SEARCH,
            CONFISCATE_DOC
        }
        #endregion

        public readonly RuleSubject subject;
        public readonly RuleCondition condition;
        public readonly RuleAction action;
        public readonly string description;
        public readonly int index;

        public Rule(int _index, string _description, RuleSubjectShortcut _subjectShortcut, RuleCondition _condition, RuleAction  _action, List<Country> countriesSubjects = null,  List<EntrantType> typesSubject = null)
        {
            index = _index;
            description = _description;
            
            switch (_subjectShortcut)
            {
                case RuleSubjectShortcut.ALL:
                    subject = new RuleSubject(new List<Country>()
                    {
                        Country.ANTEGRIA,
                        Country.ARSTOTZKA,
                        Country.IMPOR,
                        Country.KOLECHIA,
                        Country.OBRISTAN,
                        Country.REPUBLIA,
                        Country.UNITED_FEDERATION
                    });
                    break;
                
                case RuleSubjectShortcut.CITIZEN:
                    subject = new RuleSubject(new List<Country>() {Country.ARSTOTZKA, });
                    break;
                
                case RuleSubjectShortcut.FOREIGNER:
                    subject = new RuleSubject(new List<Country>()
                    {
                        Country.ANTEGRIA,
                        Country.IMPOR,
                        Country.KOLECHIA,
                        Country.OBRISTAN,
                        Country.REPUBLIA,
                        Country.UNITED_FEDERATION
                    });
                    break;
                
                case RuleSubjectShortcut.SPEC_CITIZEN:
                    subject = new RuleSubject(countriesSubjects);
                    break;
                
                case RuleSubjectShortcut.SPEC_TYPE:
                    subject = new RuleSubject(null, typesSubject, false);
                    break;
                
            }

            condition = _condition;
            action = _action;
        }
        
    }
    
    
    public static class RulesManager
    {
        public static Dictionary<Country, string[]> IssuingCities = new()
        {
            {Country.ANTEGRIA, new string[] {"St. Marmero", "Glorian", "Outer Grouse"} },
            {Country.ARSTOTZKA, new string[] {"Orvech Vonor", "East Grestin", "Paradizna"} },
            {Country.IMPOR, new string[] {"Enkyo", "Haihan", "Tsunkeido"} },
            {Country.KOLECHIA, new string[] {"Yurko City", "Vedor", "West Grestin"} },
            {Country.OBRISTAN, new string[] {"Skal", "Lorndaz", "Mergerous"} },
            {Country.REPUBLIA, new string[] {"True Glorian", "Lesrenadi", "Bostan"} },
            {Country.UNITED_FEDERATION, new string[] {"Great Rapid", "Shingleton", "Korista City"} },
        };
        
        public static Dictionary<DocumentType, string> dateFormat = new()
        {
            { DocumentType.PASSPORT, "dd.MM.yyyy" },
            { DocumentType.ID_SUPPLEMENT, "dd.MM.yyyy" },
            { DocumentType.ACCESS_PERMIT, "dd.MM.yyyy" },
            { DocumentType.GRANT_OF_ASYLUM, "dd.MM.yyyy" },
            { DocumentType.ENTRY_PERMIT, "yyyy.MM.dd" },
            { DocumentType.ID_CARD, "yyyy.MM.dd" },
            { DocumentType.ENTRY_TICKET, "yyyy.MM.dd" },
            { DocumentType.CERTIF_OF_VACCINATION, "dd.MM.yy" },
        };

        // public static List<Rule> ruleList = { new Rule(0,"Entrant must have a passport",Rule.RuleSubjectShortcut.ALL,Rule.RuleCondition.HAVE_DOCUMENT,Rule.RuleAction.ENTER) };

        // public static Dictionary<DateTime, List<DocumentType>> 

    }
}