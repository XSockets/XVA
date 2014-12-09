<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="XSocketsAzureWorkerRole" generation="1" functional="0" release="0" Id="e3ecfcd1-b366-4285-9b94-1adeda7c75a4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="XSocketsAzureWorkerRoleGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="XSocketsWorker:endpoint1" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/LB:XSocketsWorker:endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="XSocketsWorker:endpoint2" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/LB:XSocketsWorker:endpoint2" />
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
        <lBChannel name="LB:XSocketsWorker:endpoint1">
          <toPorts>
            <inPortMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker/endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:XSocketsWorker:endpoint2">
          <toPorts>
            <inPortMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker/endpoint2" />
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
          <role name="XSocketsWorker" generation="1" functional="0" release="0" software="C:\Users\Uffe\Documents\GitHub\XVA\XVA-03-06-AzureWorkerRole\AzureWorkerRole\AzureWorkerRole\csx\Debug\roles\XSocketsWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="endpoint1" protocol="tcp" portRanges="8181" />
              <inPort name="endpoint2" protocol="tcp" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="Origin" defaultValue="" />
              <aCS name="Uri" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;XSocketsWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;XSocketsWorker&quot;&gt;&lt;e name=&quot;endpoint1&quot; /&gt;&lt;e name=&quot;endpoint2&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
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
    <implementation Id="c561fcbb-256c-43fc-96e1-ec6077fdbefe" ref="Microsoft.RedDog.Contract\ServiceContract\XSocketsAzureWorkerRoleContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="e45ef5f6-42db-4f74-a9e3-a0ee02c9212f" ref="Microsoft.RedDog.Contract\Interface\XSocketsWorker:endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker:endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="b8ea4e7c-48b2-4266-a6cc-ccf362590b93" ref="Microsoft.RedDog.Contract\Interface\XSocketsWorker:endpoint2@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/XSocketsAzureWorkerRole/XSocketsAzureWorkerRoleGroup/XSocketsWorker:endpoint2" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>