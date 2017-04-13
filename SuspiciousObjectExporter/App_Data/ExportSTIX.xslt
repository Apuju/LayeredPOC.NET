<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
                xmlns:stix="http://stix.mitre.org/stix-1"
                xmlns:indicator="http://stix.mitre.org/Indicator-2"
                xmlns:stixVocabs="http://stix.mitre.org/default_vocabularies-1"
                xmlns:FileObj="http://cybox.mitre.org/objects#FileObject-2"
                xmlns:cybox="http://cybox.mitre.org/cybox-2"
                xmlns:cyboxCommon="http://cybox.mitre.org/common-2"
                xmlns:cyboxVocabs="http://cybox.mitre.org/default_vocabularies-2"
                xmlns:example="http://example.com/"
                xmlns:AddressObject="http://cybox.mitre.org/objects#AddressObject-2"
                xmlns:URIObject="http://cybox.mitre.org/objects#URIObject-2">
  <xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>
  <xsl:attribute-set name="namespace-set">
    <xsl:attribute name="xsi:schemaLocation">http://stix.mitre.org/stix-1 ../stix_core.xsd http://stix.mitre.org/Indicator-2 ../indicator.xsd http://stix.mitre.org/default_vocabularies-1 ../stix_default_vocabularies.xsd http://cybox.mitre.org/objects#FileObject-2 ../cybox/objects/File_Object.xsd http://cybox.mitre.org/default_vocabularies-2 ../cybox/cybox_default_vocabularies.xsd</xsl:attribute>
  </xsl:attribute-set>
  <xsl:template match="SuspiciousObjectList">
    <xsl:param name="guid" select="SuspiciousObject/@SYS_SystemID_Guid"></xsl:param>
    <xsl:element name="stix:STIX_Package" use-attribute-sets="namespace-set">
      <xsl:attribute name="id">
        <xsl:value-of select="concat('STIXPackage-',$guid)"/>
      </xsl:attribute>
      <xsl:attribute name="version">
        <xsl:text>1.0.1</xsl:text>
      </xsl:attribute>
      <xsl:element name="stix:STIX_Header">
        <xsl:element name="stix:Title">
          <xsl:text>Watchlist that contains Suspicious Objects</xsl:text>
        </xsl:element>
        <xsl:element name="stix:Package_Intent">
          <xsl:attribute name="xsi:type">
            <xsl:text>stixVocabs:PackageIntentVocab-1.0</xsl:text>
          </xsl:attribute>
          <xsl:text>Indicators - Watchlist</xsl:text>
        </xsl:element>
      </xsl:element>
      <xsl:element name="stix:Indicators">
        <xsl:apply-templates select="SuspiciousObject"></xsl:apply-templates>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <xsl:template match="SuspiciousObject">
    <xsl:element name="stix:Indicator">
      <xsl:attribute name="xsi:type">
        <xsl:text>indicator:IndicatorType</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="id">
        <xsl:value-of select="concat('Observable',@MD5Key)"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="@Type='0'">
          <xsl:element name="indicator:Type">
            <xsl:attribute name="xsi:type">
              <xsl:text>stixVocabs:PackageIntentVocab-1.0</xsl:text>
            </xsl:attribute>
            <xsl:text>IP Watchlist</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Description">
            <xsl:text>Blacklist - IP Address Indicator for this watchlist.</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Observable">
            <xsl:attribute name="id">
              <xsl:value-of select="concat('Observable',@MD5Key)"/>
            </xsl:attribute>
            <xsl:element name="cybox:Object">
              <xsl:element name="cybox:Properties">
                <xsl:attribute name="xsi:type">
                  <xsl:text>AddressObject:AddressObjectType</xsl:text>
                </xsl:attribute>
                <xsl:attribute name="category">
                  <xsl:text>ipv4-addr</xsl:text>
                </xsl:attribute>
                <xsl:element name="AddressObject:Address_Value">
                  <xsl:attribute name="condition">
                    <xsl:text>Equals</xsl:text>
                  </xsl:attribute>
                  <xsl:attribute name="apply_condition">
                    <xsl:text>ANY</xsl:text>
                  </xsl:attribute>
                  <xsl:value-of select="@Entity"/>
                </xsl:element>
              </xsl:element>
            </xsl:element>
          </xsl:element>
        </xsl:when>
        <xsl:when test="@Type='1'">
          <xsl:element name="indicator:Type">
            <xsl:attribute name="xsi:type">
              <xsl:text>stixVocabs:PackageIntentVocab-1.0</xsl:text>
            </xsl:attribute>
            <xsl:text>URL Watchlist</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Description">
            <xsl:text>Blacklist - URL Indicator for this watchlist.</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Observable">
            <xsl:attribute name="id">
              <xsl:value-of select="concat('Observable',@MD5Key)"/>
            </xsl:attribute>
            <xsl:element name="cybox:Object">
              <xsl:element name="cybox:Properties">
                <xsl:attribute name="xsi:type">
                  <xsl:text>URIObject:URIObjectType</xsl:text>
                </xsl:attribute>
                <xsl:element name="URIObject:Value">
                  <xsl:attribute name="condition">
                    <xsl:text>Equals</xsl:text>
                  </xsl:attribute>
                  <xsl:attribute name="apply_condition">
                    <xsl:text>ANY</xsl:text>
                  </xsl:attribute>
                  <xsl:value-of select="@Entity"/>
                </xsl:element>
              </xsl:element>
            </xsl:element>
          </xsl:element>
        </xsl:when>
        <xsl:when test="@Type='2'">
          <xsl:element name="indicator:Type">
            <xsl:attribute name="xsi:type">
              <xsl:text>stixVocabs:PackageIntentVocab-1.0</xsl:text>
            </xsl:attribute>
            <xsl:text>File Hash Watchlist</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Description">
            <xsl:text>Blacklist - Indicator that contains malicious file hashes.</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Observable">
            <xsl:attribute name="id">
              <xsl:value-of select="concat('Observable',@MD5Key)"/>
            </xsl:attribute>
            <xsl:element name="cybox:Object">
              <xsl:element name="cybox:Properties">
                <xsl:attribute name="xsi:type">
                  <xsl:text>FileObj:FileObjectType</xsl:text>
                </xsl:attribute>
                <xsl:element name="FileObj:Hashes">
                  <xsl:element name="cyboxCommon:Hash">
                    <xsl:element name="cyboxCommon:Type">
                      <xsl:attribute name="xsi:type">
                        <xsl:text>stixVocabs:PackageIntentVocab-1.0</xsl:text>
                      </xsl:attribute>
                      <xsl:text>SHA1</xsl:text>
                    </xsl:element>
                    <xsl:element name="cyboxCommon:Simple_Hash_Value">
                      <xsl:attribute name="condition">
                        <xsl:text>Equals</xsl:text>
                      </xsl:attribute>
                      <xsl:attribute name="apply_condition">
                        <xsl:text>ANY</xsl:text>
                      </xsl:attribute>
                      <xsl:value-of select="@Entity"/>
                    </xsl:element>
                  </xsl:element>
                </xsl:element>
              </xsl:element>
            </xsl:element>
          </xsl:element>
        </xsl:when>
        <xsl:when test="@Type='3'">
          <xsl:element name="indicator:Type">
            <xsl:attribute name="xsi:type">
              <xsl:text>stixVocabs:PackageIntentVocab-1.0</xsl:text>
            </xsl:attribute>
            <xsl:text>Domain Watchlist</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Description">
            <xsl:text>Blacklist - Domain Indicator for this watchlist.</xsl:text>
          </xsl:element>
          <xsl:element name="indicator:Observable">
            <xsl:attribute name="id">
              <xsl:value-of select="concat('Observable',@MD5Key)"/>
            </xsl:attribute>
            <xsl:element name="cybox:Object">
              <xsl:element name="cybox:Properties">
                <xsl:attribute name="xsi:type">
                  <xsl:text>URIObject:URIObjectType</xsl:text>
                </xsl:attribute>
                <xsl:element name="URIObject:Value">
                  <xsl:attribute name="condition">
                    <xsl:text>Equals</xsl:text>
                  </xsl:attribute>
                  <xsl:attribute name="apply_condition">
                    <xsl:text>ANY</xsl:text>
                  </xsl:attribute>
                  <xsl:value-of select="@Entity"/>
                </xsl:element>
              </xsl:element>
            </xsl:element>
          </xsl:element>
        </xsl:when>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>