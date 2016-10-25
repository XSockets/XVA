param($installPath, $toolsPath, $package, $project)

$DTE.ItemOperations.Navigate("http://xsockets.net/release-notes/?" + $package.Id + "=" + $package.Version)