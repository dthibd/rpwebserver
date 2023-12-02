Param(
    [Parameter(Mandatory)]
    [string]
    $ServerUrl
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Routes"
}

process {
    $Routes = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Routes
}
