Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl
)


begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Routes/Ids"
}

process {
    $Routes = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Routes
}
