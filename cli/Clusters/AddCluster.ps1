Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl,
    [Parameter(Mandatory)]
    [Hashtable]
    $Cluster
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Cluster"

}

process {
    $Cluster | ConvertTo-Json | Invoke-RestMethod -Uri $ApiRoute -Method Post -ContentType "application/json"
}


