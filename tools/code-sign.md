## VSIX code signing

This is notes about using code signing for a VSIX (and also with NuGet packages, not doing this at the moment, to bound to local PC)

### VSIX extension code signing

- Download built .vsix from VSIX Gallery

- Log in to SimplySign Desktop

- Run sign (.NET global tool)

`sign code certificate-store -cfp 0Fxxx -k DEEDXXXX -csp "SimplySign CSP" -t http://time.certum.pl "EF Core Power Tools v2.6.750.vsix" -fl fileslist.txt`

- Upload to MarketPlace

## Notes

fileslist.txt contents:
EFCorePowerTools.dll
RevEng.Common.dll

-k = Key container id and -csp both displayed by certutil:

`certutil -user -store my "<certificate serial number>"`

-cfp = cert fingerprint, to get, save as binary .cer, then run this PowerShell command:

`Get-FileHash -Algorithm SHA256 <path to .cer file> | Format-Table -AutoSize`

## Certificate renewal notes

- 365 day cert must be bought again every year
https://certmanager.certum.pl/dashboard 
- select automatic identity validation, using face recognition
- required documents: 
  - utility bill with my name and address on it
  - PDF file with link to GitHub profile (profile has my full name on it)

## Nupkg code signing

dotnet nuget sign "ErikEJ.EntityFramework.SqlServer.6.6.7.nupkg" --certificate-fingerprint xxxx --timestamper http://time.certum.pl

--certificate-fingerprint = certificate thumbprint vist i Certmgr/SimplySign
