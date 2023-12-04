Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Clusters/Ids"
}

process {
    $Routes = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Routes
}
