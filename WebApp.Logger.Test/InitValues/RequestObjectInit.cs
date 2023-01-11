﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;

namespace WebApp.Logger.Test.Extensions
{
    public static class RequestObjectInit
    {
        public static object GetRequestObject()
        {
            string json = "{\r\n  \"userId\": null,\r\n  \"application\": null,\r\n  \"ipAddress\": \"127.0.0.1\",\r\n  \"version\": \"https\",\r\n  \"host\": \"localhost:44373\",\r\n  \"url\": \"POST https://localhost:44373/api/Blog/add\",\r\n  \"source\": null,\r\n  \"form\": \"\",\r\n  \"body\": {\r\n           \"name\":\"My Blog\",\r\n           \"description\":\"My blog description\",\r\n           \"motto\":\"Blog Motto\",\r\n           \"posts\":null,\r\n           \"id\":0},\r\n  \"response\": {\r\n                \"statusCode\":200,\r\n                \"appStatusCode\":\"AP1300E\",\r\n                \"isSuccess\":true,\r\n                \"message\":null,\r\n                \"data\":{\"name\":\"My Blog\",\"description\":\"My blog description\",\"motto\":\"Blog Motto\",\"posts\":[],\"id\":310},\"errors\":null},\r\n  \"requestHeaders\": {\r\n                \"Accept\":[\"*/*\"],\r\n                \"Accept-Encoding\":[\"gzip, deflate, br\"],\r\n                \"Accept-Language\":[\"en-US,en;q=0.9\"],\r\n                \"Connection\":[\"close\"],\r\n                \"Content-Length\":[\"95\"],\r\n                \"Content-Type\":[\"application/json-patch\\\\u002Bjson\"],\r\n                \"Host\":[\"localhost:44373\"],\r\n                \"Referer\":[\"https://localhost:44373/swagger/index.html\"],\r\n                \"User-Agent\":[\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36\"],\r\n                \"sec-ch-ua\":[\"\\\\u0022Not?A_Brand\\\\u0022;v=\\\\u00228\\\\u0022, \\\\u0022Chromium\\\\u0022;v=\\\\u0022108\\\\u0022, \\\\u0022Google Chrome\\\\u0022;v=\\\\u0022108\\\\u0022\"],\r\n                \"sec-ch-ua-mobile\":[\"?0\"],\r\n                \"sec-ch-ua-platform\":[\"\\\\u0022Windows\\\\u0022\"],\r\n                \"origin\":[\"https://localhost:44373\"],\r\n                \"sec-fetch-site\":[\"same-origin\"],\r\n                \"sec-fetch-mode\":[\"cors\"],\r\n                \"sec-fetch-dest\":[\"empty\"]},\r\n  \"responseHeaders\": {\r\n                \"Accept\":[\"*/*\"],\r\n                \"Accept-Encoding\":[\"gzip, deflate, br\"],\r\n                \"Accept-Language\":[\"en-US,en;q=0.9\"],\r\n                \"Connection\":[\"close\"],\r\n                \"Content-Length\":[\"95\"],\r\n                \"Content-Type\":[\"application/json-patch\\\\u002Bjson\"],\r\n                \"Host\":[\"localhost:44373\"],\r\n                \"Referer\":[\"https://localhost:44373/swagger/index.html\"],\r\n                \"User-Agent\":[\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36\"],\r\n                \"sec-ch-ua\":[\"\\\\u0022Not?A_Brand\\\\u0022;v=\\\\u00228\\\\u0022, \\\\u0022Chromium\\\\u0022;v=\\\\u0022108\\\\u0022, \\\\u0022Google Chrome\\\\u0022;v=\\\\u0022108\\\\u0022\"],\r\n                \"sec-ch-ua-mobile\":[\"?0\"],\"sec-ch-ua-platform\":[\"\\\\u0022Windows\\\\u0022\"],\r\n                \"origin\":[\"https://localhost:44373\"],\r\n                \"sec-fetch-site\":[\"same-origin\"],\r\n                \"sec-fetch-mode\":[\"cors\"],\r\n                \"sec-fetch-dest\":[\"empty\"]},\r\n  \"scheme\": \"https\",\r\n  \"traceId\": \"40000133-0000-f400-b63f-84710c7967bb\",\r\n  \"proctocol\": \"HTTP/2\",\r\n  \"area\": null,\r\n  \"controllerName\": null,\r\n  \"actionName\": null,\r\n  \"executionDuration\": 0.0,\r\n  \"roleId\": null,\r\n  \"languageId\": null,\r\n  \"isFirstLogin\": null,\r\n  \"loggedInDateTimeUtc\": null,\r\n  \"loggedOutDateTimeUtc\": null,\r\n  \"loginStatus\": null,\r\n  \"pageAccessed\": null,\r\n  \"sessionId\": null,\r\n  \"urlReferrer\": null,\r\n  \"statusCode\": 200,\r\n  \"appStatusCode\": \"*****\",\r\n  \"createdDateUtc\": null\r\n}";
            return json.ToModel<object>();
        }
    }
}