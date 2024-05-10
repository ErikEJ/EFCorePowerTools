<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes"/>

  <!--WOW. this is incredibly lame to parse out a date. /ugh xslt 1.0 :( -->
  <!--Example date: 2017-05-24T12:50:26.6582201-05:00 -->
  <xsl:template name="formatdate">
    <xsl:param name="DateTimeStr" />

    <xsl:variable name="datestr">
      <xsl:value-of select="substring-before($DateTimeStr,'T')" />
    </xsl:variable>

    <xsl:variable name="timeStart">
      <xsl:value-of select="substring-after($DateTimeStr,'T')" />
    </xsl:variable>

    <xsl:variable name="timestr">
      <xsl:value-of select="substring-before($timeStart,'.')" />
    </xsl:variable>

    <xsl:variable name="hours">
      <xsl:value-of select="substring($timestr,1,2)" />
    </xsl:variable>

    <xsl:variable name="minutes">
      <xsl:value-of select="substring($timestr,4,2)" />
    </xsl:variable>

    <xsl:variable name="mm">
      <xsl:value-of select="substring($datestr,6,2)" />
    </xsl:variable>

    <xsl:variable name="dd">
      <xsl:value-of select="substring($datestr,9,2)" />
    </xsl:variable>

    <xsl:variable name="yyyy">
      <xsl:value-of select="substring($datestr,1,4)" />
    </xsl:variable>

    <xsl:value-of select="concat($mm,'/', $dd, '/', $yyyy, ' ', $hours, ':', $minutes)" />
  </xsl:template>

  <xsl:template match="/">
    <html>
      <head>
        <title>
          DB Project: <xsl:value-of select="/Report/Issues/Project[1]/@Name"/>
        </title>
        <style type="text/css">
          body {
          background-color: #3e94ec;
          font-family: "Consolas", helvetica, arial, sans-serif;
          /*font-size: 16px;*/
          font-weight: 400;
          text-rendering: optimizeLegibility;
          }

          h1, h2, h3, hr {
          max-width: 90%;
          width: 90%;
          text-align:center;
          }

          div.table-title {
          display: block;
          margin: auto;
          max-width: 90%;
          padding: 5px;
          width: 90%;
          }

          .table-title h3 {
          color: #fafafa;
          font-size: 30px;
          font-weight: 400;
          font-style: normal;
          font-family: "Consolas", helvetica, arial, sans-serif;
          /*text-shadow: -1px -1px 1px rgba(0, 0, 0, 0.1);*/
          text-transform: uppercase;
          }


          /*** Table Styles **/

          .table-fill {
          background: white;
          border-radius: 10px;
          border-collapse: collapse;
          height: 320px;
          margin: auto;
          max-width: 90%;
          padding: 5px;
          width: 90%;
          box-shadow: 0 5px 10px rgba(0, 0, 0, 0.1);
          animation: float 5s infinite;
          }

          .table-fill caption{
          color: #D5DDE5;
          background: #1b1e24;
          border-bottom: 4px solid #9ea7af;
          font-size: 18px;
          font-weight: bold;
          padding: 12px;
          text-align: left;
          /*text-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);*/
          vertical-align: middle;
          border-top-left-radius: 10px;
          border-top-right-radius: 10px;
          }

          th {
          color: #D5DDE5;
          background: #1b1e24;
          border-bottom: 4px solid #9ea7af;
          border-right: 1px solid #343a45;
          font-size: 14px;
          font-weight: 100;
          padding: 12px;
          text-align: left;
          /*text-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);*/
          vertical-align: middle;
          }

          th:first-child {
          border-top-left-radius: 3px;
          }

          th:last-child {
          border-top-right-radius: 3px;
          border-right: none;
          }

          tr {
          display: table-row;
          border-top: 1px solid #C1C3D1;
          border-bottom-: 1px solid #C1C3D1;
          color: #666B85;
          font-size: 16px;
          font-weight: normal;
          /*text-shadow: 0 1px 1px rgba(256, 256, 256, 0.1);*/
          }

          tr:hover td {
          background: #4E5066;
          color: #FFFFFF;
          border-top: 1px solid #22262e;
          border-bottom: 1px solid #22262e;
          }

          tr:first-child {
          border-top: none;
          }

          tr:last-child {
          border-bottom: none;
          }

          tr:nth-child(odd) td {
          background: #EBEBEB;
          }

          tr:nth-child(odd):hover td {
          background: #4E5066;
          }

          tr:last-child td:first-child {
          border-bottom-left-radius: 10px;
          }

          tr:last-child td:last-child {
          border-bottom-right-radius: 10px;
          }

          td {
          background: #FFFFFF;
          padding: 8px;
          text-align: left;
          vertical-align: middle;
          font-weight: 300;
          font-size: 12px;
          /*text-shadow: -1px -1px 1px rgba(0, 0, 0, 0.1);*/
          border-right: 1px solid #C1C3D1;
          white-space:normal;
          }

          td:last-child {
          border-right: 0px;
          }

          th.text-left {
          text-align: left;
          }

          th.text-center {
          text-align: center;
          }

          th.text-right {
          text-align: right;
          }

          td.text-left {
          text-align: left;
          }

          td.text-center {
          text-align: center;
          }

          td.text-right {
          text-align: right;
          }

          td.object {
          max-width:26em;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          }

          td.message {
          white-space:normal !important;
          overflow: hidden;
          text-overflow: ellipsis;
          }

          /*TOOLTIP*/
          .has-tooltip {
          /*position: relative;*/
          /*display: inline;*/
          }

          .tooltip-wrapper {
          position: absolute;
          visibility: hidden;
          }

          .has-tooltip:hover .tooltip-wrapper {
          visibility: visible;
          opacity: 0.8;
          /*top: 30px;*/
          /*left: 50%;*/
          /*margin-left: -76px;*/
          /* z-index: 999; defined above with value of 5 */
          }

          .tooltip {
          display: block;
          position: relative;
          top: 2em;
          right: 60%;
          /*width: 140px;*/
          height: 32px;
          line-height: 32px;
          /*margin-left: -76px;*/
          color: #FFFFFF;
          background: #000000;
          text-align: center;
          border-radius: 8px;
          /*box-shadow: 4px 3px 10px #ff5e00;*/
          padding-left: 10px;
          padding-right: 10px;
          }

          .tooltip:after {
          content: '';
          position: absolute;
          bottom: 100%;
          left: 20px;
          margin-left: -8px;
          width: 0;
          height: 0;
          border-bottom: 8px solid #000000;
          border-right: 8px solid transparent;
          border-left: 8px solid transparent;
          }
        </style>
      </head>
      <body>
        <h1>SQL Server Rules</h1>
        <h3>
          Report Date: <xsl:call-template name="formatdate">
            <xsl:with-param name="DateTimeStr" select="/Report/Information/ReportDate"/>
          </xsl:call-template>
        </h3>

        <table class="table-fill">
          <caption>
            Database Project: <xsl:value-of select="/Report/Issues/Project[1]/@Name"/>
          </caption>
          <thead>
            <th class="text-right">Row</th>
            <th class="text-left">Rule Id</th>
            <th class="text-left">Object</th>
            <th class="text-left">Line</th>
            <th class="text-left">Offset</th>
            <th class="text-left">Rule Message</th>
          </thead>
          <tbody>
            <xsl:for-each select="/Report/Issues/Project[1]/Issue">
              <xsl:sort select="./@File" data-type="text"/>
              <xsl:sort select="./@Line" data-type="number"/>
              <xsl:sort select="./@Offset" data-type="number"/>
              <tr>
                <td class="text-right">
                  <xsl:value-of select="position()"/>
                </td>
                <td class="text-left">
                  <xsl:value-of select="./@TypeId"/>
                </td>
                <td class="text-left object has-tooltip">
                  <xsl:value-of select="./@File"/>
                  <span class="tooltip-wrapper">
                    <span class="tooltip">
                      <xsl:value-of select="./@File"/>
                    </span>
                  </span>
                </td>
                <td class="text-left">
                  <xsl:value-of select="./@Line"/>
                </td>
                <td class="text-left">
                  <xsl:value-of select="./@Offset"/>
                </td>
                <td class="text-left message">
                  <xsl:value-of select="./@Message"/>
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>

        <br />
        <br />
        <table class="table-fill">
          <caption>Enforced Rules List</caption>
          <thead>
            <th class="text-right">Row</th>
            <th class="text-left">Rule Id</th>
            <th class="text-left">Category</th>
            <th class="text-left">Description</th>
          </thead>
          <tbody>
            <xsl:for-each select="/Report/IssueTypes/IssueType">
              <xsl:sort select="./@Category" data-type="text" order="descending"/>
              <xsl:sort select="./@Id" data-type="text"/>
              <tr>
                <td class="text-right">
                  <xsl:value-of select="position()"/>
                </td>
                <td class="text-left">
                  <xsl:value-of select="./@Id"/>
                </td>
                <td class="text-left">
                  <xsl:value-of select="./@Category"/>
                </td>
                <td class="text-left" style="white-space:normal;">
                  <xsl:value-of select="./@Description"/>
                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
