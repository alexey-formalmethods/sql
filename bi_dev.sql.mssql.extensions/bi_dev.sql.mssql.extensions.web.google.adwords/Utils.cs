using Google.Api.Ads.AdWords.Lib;
using Google.Api.Ads.AdWords.v201809;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.web.google.adwords
{
    public static class Utils
    {
        private static ReportDefinitionReportType? StringToReportDefinitionReportType(string reportType)
        {
            ReportDefinitionReportType reportTypeEnum;
            var result = Enum.TryParse(reportType, out reportTypeEnum);
            if (result)
            {
                return reportTypeEnum;
            }
            else
            {
                return null;
            }
        }
        public class ReportTypeResult
        {
            private ReportDefinitionReportType? reportType;
            public string ReportType { get { return this.reportType?.ToString(); } }
            public ReportDefinitionField[] ReportDefinitionFields { get; set; }
            public bool IsFound { get; set; }
            public ReportTypeResult(ReportDefinitionReportType reportType, ReportDefinitionField[] reportDefinitionFields)
            {
                this.reportType = reportType;
                this.ReportDefinitionFields = reportDefinitionFields;
                this.IsFound = true;
            }
            public ReportTypeResult()
            {
                this.IsFound = false;
            }
        }
        public static string [] GetReports()
        {
            return Enum.GetNames(typeof(ReportDefinitionReportType));
        }
        public static string GetReportDefinition(string refreshToken, string developerToken, string clientId, string clientSecret, string reportType, bool nullWhenError)
        {
            try
            {
                ReportTypeResult result;
                AdWordsUser user = new AdWordsUser();
                user.Config.OAuth2RefreshToken = refreshToken;
                user.Config.OAuth2ClientId = clientId;
                user.Config.OAuth2ClientSecret = clientSecret;

                using (var rservice = (ReportDefinitionService)user.GetService(AdWordsService.v201809.ReportDefinitionService))
                {

                    rservice.RequestHeader.developerToken = developerToken;
                    var reportTypeEnum = StringToReportDefinitionReportType(reportType);
                    if (reportTypeEnum.HasValue)
                    {
                        result = new ReportTypeResult(
                            reportTypeEnum.Value,
                            rservice.getReportFields(reportTypeEnum.Value)
                        );
                    }
                    else
                    {
                        result = new ReportTypeResult();
                    }
                }
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception e)
            {
                return Common.ThrowIfNeeded<string>(e, nullWhenError);
            }
        }
        private static string[] ParseJsonFields(string json)
        {
            string[] result;
            try
            {
                result = JsonConvert.DeserializeObject<List<JObject>>(json).Select(x => x.Properties().FirstOrDefault().Value.ToString()).Distinct().ToArray();

            }
            catch
            {
                try
                {
                    result = JsonConvert.DeserializeObject<string[]>(json).Distinct().ToArray();
                }
                catch
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<JObject>(json).Properties().Select(x => x.Value.ToString()).Distinct().ToArray();
                    }
                    catch
                    {
                        try
                        {
                            result = json.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).Distinct().ToArray();
                        }
                        catch
                        {
                            result = new string[] { json };
                        }
                    }
                }
            }
            return result;
        }
        public static string GetAds(string refreshToken, string developerToken, string clientId, string clientSecret, string clientCustomerId, string selectorFieldsJson, string AdIdJson, bool nullWhenError)
        {
            AdWordsUser user = new AdWordsUser();
            user.Config.OAuth2RefreshToken = refreshToken;
            user.Config.OAuth2ClientId = clientId;
            user.Config.OAuth2ClientSecret = clientSecret;

            string[] selectorFields = ParseJsonFields(selectorFieldsJson);
            string[] ids = ParseJsonFields(AdIdJson);
            // Create the required service.
            using (AdService service = (AdService)user.GetService(AdWordsService.v201809.AdService))
            {
                service.RequestHeader.clientCustomerId = clientCustomerId;
                service.RequestHeader.developerToken = developerToken;
                try
                {
                    Selector s = new Selector();
                    s.fields = selectorFields;
                    s.predicates = new Predicate[1];
                    s.predicates[0] = new Predicate();
                    s.predicates[0].field = Ad.Fields.Id;
                    s.predicates[0].@operator = PredicateOperator.IN;
                    s.predicates[0].values = ids;
                    var e = service.get(s);
                    return "kaka";
                }
                catch (Exception e)
                {
                    return Common.ThrowIfNeeded<string>(e, nullWhenError);
                }
            }
    }
    }
}
