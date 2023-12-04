Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Clusters"
}

process {
    $Routes = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Routes
}
