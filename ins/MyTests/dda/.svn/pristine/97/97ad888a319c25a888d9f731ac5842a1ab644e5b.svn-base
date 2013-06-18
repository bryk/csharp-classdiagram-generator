using System.IO;
using System.Text.RegularExpressions;
using DocumentExtractor.Model;
using System;

namespace DocumentExtractor.Core
{
    public class Extractor
    {
        public static Document ReadStream(StreamReader fileReader)
        {
            var content = fileReader.ReadToEnd();
            fileReader.Close();

            var body = new Regex(@"(?<=(.|\n)*?<body.*?>(\n)*)(.|\n)*(?=(\n)*</body>)");
            var comments = new Regex(@"(\n)*<!--(.|\n)*?-->(\n)*");
            var scripts = new Regex(@"(\n)*<script(.|\n)*?</script>(\n)*");
            content = scripts.Replace(comments.Replace(body.Match(content).Value, ""), "");

            var tables = Regex.Split(content, @"\s*</table>\s*<table.*?>\s*");
            content = tables[2];

            var titles = Regex.Split(tables[1], @"</a>\s*->\s*<a.*?>");
            string section, topic;
            if(titles.Length == 4)
            {
                section = titles[2];
                topic = Regex.Replace(Regex.Replace(titles[3], @"(.|\s)*<strong>\s*", ""), @"\s*</strong>(.|\n)*", "");
            } 
            else
            {
                section = Regex.Replace(titles[2], @"\s*<strong>(.|\n)*", "");
                topic = Regex.Replace(Regex.Replace(titles[2], @"(.|\s)*<strong>\s*", ""), @"\s*</strong>(.|\n)*", "");
            }

            var names = Regex.Matches(content, "(?<=<td.*?><span class=\"name\"><a.*?></a><b>).*?(?=</b></span>)");
            var datesAndBodies = Regex.Matches(content, "(?<=<span class=\"postbody\">)(.|\\s)*?(?=((&nbsp;){3})?</span>)");

            var document = new Document
                                {
                                    SectionTitle = section,
                                    TopicTitle = topic
                                };

            for (var i = 0; i < names.Count; ++i )
            {
                if(names[i].Value.Equals("Google"))
                {
                    continue;
                }

                content = Regex.Replace(datesAndBodies[2 * i + 1].Value, @"<br\s?/?>", "");

                var dateString = Regex.Match(datesAndBodies[2*i].Value, @"\d{2} [A-Z][a-z]{2} \d{4} \d{2}:\d{2}").Value;
                DateTime date;
                if (!DateTime.TryParse(dateString, out date))
                {
                    continue;
                }

                var post = new Post
                               {
                                   SectionTitle = section,
                                   TopicTitle = topic,
                                   Author = names[i].Value,
                                   Date = date,
                                   Content = content.Trim().Replace("\t", ""),
                                   Id = names[i].Value + date
                               };
                document.Posts.Add(post);
            }

            return document;
        }
    }
}
