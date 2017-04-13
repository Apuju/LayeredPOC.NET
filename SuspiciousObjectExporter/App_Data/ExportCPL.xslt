<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" omit-xml-declaration="yes" indent="yes"/>
  <xsl:template match="SuspiciousObjectList">
    <xsl:text>define category sandbox_feedback_blacklists</xsl:text>
    <xsl:text>&#10;</xsl:text>
    <xsl:apply-templates select="SuspiciousObject" />
    <xsl:text>End</xsl:text>
  </xsl:template>
  <xsl:template match="SuspiciousObject">
    <xsl:value-of select="@Entity"/>
    <xsl:text>&#10;</xsl:text>
  </xsl:template>
</xsl:stylesheet>