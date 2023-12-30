# OSU b222 bancho

### Whats working?
```
Basic Authenticating without database
Loading Global Scores(currently random score without database)
Loading Players Stats(currently server setting because no db)
```
### TODO

~~Move everything to mysql~~ DONE
~~Add score submitting~~ DONE
make api
make frontend


### How to Compile, if you don't want just download build from releases

1. Download Repo, and open .sln in visual studio
2. Download the <a href="https://github.com/Zordon1337/LibHTTP">LibHTTP</a> and compile it/download from releases
3. Add LibHTTP to References
4. Compile Server(Recommended configuration is DEBUG|X86)
5. go to compiled binary and run it
6. now u need to redirect the osu endpoints to localhost
there are several ways to do that such as Fiddler Script, Hosts file, Patching Client
but the easiest one is to just use <a href="https://github.com/minisbett/ultimate-osu-server-switcher">ultimate-osu-server-switcher</a>


### Screenshots
![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/6f14b02b-89d9-44fe-84bb-089f0cf3b70b)
<br/>
![image](https://github.com/Zordon1337/LegacyBancho/assets/65111609/cfc689c0-7737-44ec-b58a-941e6ece5622)

