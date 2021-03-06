<h1>FastDecryptApp for MTI</h1>
<p> Notes: </p>
<ul>
    <li>FastDecryptApp1.8.7 is the most stable release. 2.0.0 works great for the -a flag.</li>
    <li>Always use 1.8.7 for any root directories ( i.e. FastDecryptApp1.8.7 -d keypath c:\ )</li>
    <li>You can use 2.0.0 for network shares if the user has access to the share.</li>
    <li>For older OS like Server 2003 use FastDecryptDN35.exe (for use on systems with win32 error)</li>
</ul>
<hl>
<h3>Flags for FastDecryptApp1.8.7.exe</h3>
<ul>
    <li>-d : decrypts and deletes, will not delete a .weapologize if the decrypted version is present.</li>
    <li>-a : analysis mode, will report stats for the directory specified.</li>
    <li>-e : decrypt only, will leave .weapologize files. (use inconjunction with -d, see below)</li>
    <li>-c : count only, counts every file in a given directory</li>
    <li>-k : gives the keyname of a file</li>
</ul>

<h3>Examples</h3>
<p>
    <code>FastDecryptApp.exe -d c:\path\to\key.keyxml c:\ </code> <br>
    <code>FastDecryptApp.exe -a d:\ </code> <br>
    <code>FastDecryptApp.exe -e c:\path\to\key.keyxml y:\ ... then ... FastDecryptApp.exe -d c:\path\to\key.keyxml y:\ </code><br>
    <code>FastDecryptApp.exe -c c:\ </code> <br>
    <code>FastDecryptApp.exe -k c:\users\user\desktop\one.txt </code>
</p>
