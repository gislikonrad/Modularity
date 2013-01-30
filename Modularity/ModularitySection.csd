<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="c9f3c23b-b904-4f1e-8960-f3639aaad252" namespace="Modularity" xmlSchemaNamespace="urn:Modularity" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="ModularitySection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="modularity">
      <elementProperties>
        <elementProperty name="CustomModules" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="customModules" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/c9f3c23b-b904-4f1e-8960-f3639aaad252/ModuleElementCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElementCollection name="ModuleElementCollection" xmlItemName="add" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/c9f3c23b-b904-4f1e-8960-f3639aaad252/ModuleElement" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ModuleElement">
      <attributeProperties>
        <attributeProperty name="Type" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="type" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/c9f3c23b-b904-4f1e-8960-f3639aaad252/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>