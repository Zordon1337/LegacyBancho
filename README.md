![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/9d9c2854-d062-4581-8196-5058960008c0)# OSU b222 bancho

### Whats working?
```
Logging in and verifying password with database
Global Scores
Loading Players Stats(From database)
submiting scores
```
### TODO
~~Move everything to mysql~~ DONE <br/>
~~Add score submitting~~ DONE <br/>
make api <br/>
make frontend <br/>
Documentation of Osu! b222 protcol <br/>
Make server stable <br/>
Add Replay Handler(sending on submit, and accessing it from global tops)<br/>
~~Make beatmap status(Ranked, non ranked, waiting, need to update)~~ <br/>
Add Total Accuracy<br/>
Implement Basic IRC<br/>


### How to Compile, if you don't want just download build from releases

1. Download Repo, and open .sln in visual studio
2. Download the <a href="https://github.com/Zordon1337/LibHTTP">LibHTTP</a> and compile it/download from releases
3. Add LibHTTP to References
4. download Mysql.data package from NuGET
5. Compile Server(Recommended configuration is DEBUG|X64)
6. go to compiled binary and run it
7. now u need to redirect the osu endpoints to localhost
there are several ways to do that such as Fiddler Script, Hosts file, Patching Client
but the easiest one is to just use <a href="https://github.com/minisbett/ultimate-osu-server-switcher">ultimate-osu-server-switcher</a>
8. go to SERVER_IP:80/register to create your account
9. open osu and enjoy
ps: db scheme comes with 2 maps ranked
https://osu.ppy.sh/beatmapsets/118 - Every level ranked
https://osu.ppy.sh/beatmapsets/1 - every level ranked

### Screenshots
![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/630bf3d6-8b04-42ea-85e1-6e6e38c2c524)
<br/>
![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/b28380a8-3809-44ab-8df4-f2ec22c10550)
<br/>
![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/5f1905e5-6bd7-4201-97e3-00d961b49dd6)
<br/>
![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/a261ef08-3a4c-4ac7-a5be-f5a9e54490ab)
<br/>




