<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="XSocketsAzureWorkerRole" generation="1" functional="0" release="0" Id="56e5325c-33e5-4443-8ac1-04d53ef75a3c" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="XSocketsAzureWorkerRoleGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="XSocketsWorker:Endpoint" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/LB:XSocketsWorker:Endpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="XSocketsWorker:Origin" defaultValue="">
          <maps>
            <mapMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/MapXSocketsWorker:Origin" />
          </maps>
        </aCS>
        <aCS name="XSocketsWorker:Uri" defaultValue="">
          <maps>
            <mapMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/MapXSocketsWorker:Uri" />
          </maps>
        </aCS>
        <aCS name="XSocketsWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/MapXSocketsWorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:XSocketsWorker:Endpoint">
          <toPorts>
            <inPortMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker/Endpoint" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapXSocketsWorker:Origin" kind="Identity">
          <setting>
            <aCSMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker/Origin" />
          </setting>
        </map>
        <map name="MapXSocketsWorker:Uri" kind="Identity">
          <setting>
            <aCSMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker/Uri" />
          </setting>
        </map>
        <map name="MapXSocketsWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="XSocketsWorker" generation="1" functional="0" release="0" software="C:\slasken\XSocketsAzureWorkerRole\XSocketsAzureWorkerRole\csx\Release\roles\XSocketsWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint" protocol="tcp" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="Origin" defaultValue="" />
              <aCS name="Uri" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;XSocketsWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;XSocketsWorker&quot;&gt;&lt;e name=&quot;Endpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="XSocketsWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="XSocketsWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="XSocketsWorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="4248ba55-c676-420c-abd3-22a4312546ab" ref="Microsoft.RedDog.Contract\ServiceContract\XSocketsAzureWorkerRoleContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="4c036d88-1b1c-43f5-84c6-c6d810c923f7" ref="Microsoft.RedDog.Contract\Interface\XSocketsWorker:Endpoint@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker:Endpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>