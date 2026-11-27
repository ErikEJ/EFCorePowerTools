# Remove retired tool ZIPs from Git history

This implements [proposal 1 of issue #3552](https://github.com/ErikEJ/EFCorePowerTools/issues/3552#issuecomment-6512522601): remove only the six retired `src/GUI/lib/efreveng*.exe.zip` paths enumerated in `remove-retired-zips.cs`. The 80, 90, and 100 ZIPs remain tracked, including their history. Build and packaging behavior is unchanged.

This is a one-time repository maintenance operation. Merging this script in a PR does **not** remove existing Git history. A maintainer must coordinate replacement of the upstream branches and tags.

## Prepare and verify

Requirements: [.NET 10 SDK or later](https://learn.microsoft.com/dotnet/core/sdk/file-based-apps), Git, Python 3 (required by git-filter-repo), and [git-filter-repo](https://github.com/newren/git-filter-repo) (tested with 2.47.0). Run from Bash, PowerShell, or another shell with these tools available. Reserve space for the original mirror and its rewritten objects during repacking.

1. Coordinate a push freeze and record the upstream branch/tag protections. Preserve a complete, untouched backup mirror outside the repository being rewritten:

   ```sh
   git clone --mirror https://github.com/ErikEJ/EFCorePowerTools.git EFCorePowerTools-backup.git
   ```

2. Run the script against that backup, using a destination that does not exist:

   ```sh
   dotnet run --file tools/remove-retired-zips.cs -- ./EFCorePowerTools-backup.git ./EFCorePowerTools-clean.git
   ```

   Alternatively, pass `--filter-repo /path/to/git-filter-repo` for a standalone downloaded git-filter-repo script. The C# wrapper invokes it with `python3` on Unix or `python` on Windows; use `--python /path/to/python` to override the interpreter. With git-filter-repo installed on PATH, neither option is needed. Local clones may hard-link immutable Git objects to save disk space; the cleanup only rewrites the new mirror. Keep the backup until the migration is confirmed.

3. Review `EFCorePowerTools-clean.git/filter-repo/cleanup-report.json`, `commit-map`, and `ref-map`. The script checks all reference tips for identical non-target file names, modes, and blob IDs, checks that none of the six paths occurs in reachable history, and runs `git fsck --full`. It records before/after references and packed sizes. Any failure stops before publication; retain the failed mirror for inspection and use a new destination for another attempt.

   A local checkout can be used as the source for rehearsal. It only includes locally available refs: it is not a replacement for a fresh upstream mirror, especially for release tags and branches absent from a fork.

## Publish during the coordinated maintenance window

After review, a maintainer with permission to replace upstream history should temporarily allow force updates on the affected branches and tags, then publish from the verified mirror:

```sh
git -C EFCorePowerTools-clean.git remote add publish git@github.com:ErikEJ/EFCorePowerTools.git
git -C EFCorePowerTools-clean.git push --force publish 'refs/heads/*:refs/heads/*' 'refs/tags/*:refs/tags/*'
```

Confirm upstream refs have not changed since the backup before running this command. Push only branches and tags; do not use `push --mirror`, which could try to alter GitHub-managed refs or publish local remote-tracking refs. Restore protections afterward and verify a fresh upstream clone with `git fsck --full`, the six-path history check, and `git count-objects -vH`.

Rewritten commits and tags acquire new IDs; signatures cannot survive rewriting. Tell contributors to make fresh clones and transplant unpublished work onto the new history instead of merging the old history back. Existing PRs, forks, and GitHub-managed PR refs may still retain old objects. GitHub's server-side retention and garbage collection determine when the hosting storage shrinks; the local packed-size reduction does not guarantee immediate server reclamation. Coordinate with GitHub if retained refs prevent the expected clone-size reduction.

If rollback is needed, coordinate another push freeze and restore branches and tags from the untouched backup. The script never publishes or changes the source repository.
