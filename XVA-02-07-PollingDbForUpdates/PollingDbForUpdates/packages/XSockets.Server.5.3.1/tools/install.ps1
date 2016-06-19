param($installPath, $toolsPath, $package, $project)

$DTE.ItemOperations.Navigate("https://xsockets.net/release-notes/?" + $package.Id + "=" + $package.Version)