﻿namespace API.Utils
{
    public class InsTemplateHtml
    {
        public string Html = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n  <title>InsTemplate</title>\r\n  <style>   \r\n    table,\r\n    th {\r\n      border: 1px solid #635bff;\r\n    }\r\n  </style>\r\n</head>\r\n\r\n<body style=\"border: 5px; border-color: #635bff; border-style: solid; padding: 50px; border-width: 0.2em;\">\r\n  <div style=\"text-align: center;\">\r\n    <h1 style=\"color: #635bff;\">[V] insurance</h1>\r\n    <h3>Copy of the Insurance policy document</h3>\r\n    <h2 style=\"padding-bottom: 10px;\">{{Type}} {{InsType}} Insurance Plan</h2>\r\n  </div>\r\n  <div style=\"padding-bottom: 10px;\">Vehicle Type - {{Type}}</div>\r\n  <div style=\"padding-bottom: 10px;\">Vehicle Model - {{Model}}</div>\r\n  <div style=\"padding-bottom: 10px;\">Registration no - {{RegNo}}</div>\r\n  <div style=\"padding-bottom: 10px;\">Name of the Insurer - {{fullName}}</div>\r\n  <div style=\"padding-bottom: 10px; display: flex; justify-content:space-around;\">Policy Coverage start date:\r\n    {{StartDate}} and end date: {{EndDate}}</div>\r\n  <table style=\"width:100%;\">\r\n    <tr>\r\n      <th>Accident coverage</th>\r\n      <td>{{AccCover}}</td>\r\n      <th>Current policy status</th>\r\n      <td>{{Status}}</td>\r\n    </tr>\r\n    <tr>\r\n      <th>Selected Insurance Plan</th>\r\n      <td>{{InsType}}</td>\r\n      <th>No of Years</th>\r\n      <td>{{Years}}</td>\r\n    </tr>\r\n    <tr>\r\n      <th>Premium amount</th>\r\n      <td>{{Premium}}</td>\r\n      <th>GST (18%)</th>\r\n      <td>{{GST}}</td>\r\n    </tr>\r\n  </table>\r\n  <div style=\"text-align: center; padding-top: 10px;\">Total: {{Total}}</div>\r\n  <h4>Details of the Insurer below:</h4>\r\n  <div>Name - {{fullName}}</div>\r\n  <div>Email - {{email}}</div>\r\n  <div>Phone No - {{phone}}</div>\r\n  <div>Pincode - {{pincode}}</div>\r\n  </div>\r\n</body>\r\n\r\n</html>";
    }
}