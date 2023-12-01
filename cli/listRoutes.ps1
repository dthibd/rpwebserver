Param(
    [Parameter()]
    [string]
    $ServerUrl = "https://localhost:8101"
)

begin {
    $ApiRoute = "$($ServerUrl)/ReverseProxy/Routes"
}

process {
    $Routes = Invoke-RestMethod -Uri $ApiRoute -Method Get

    $Routes
}

end {

}
