-- premake5.lua
workspace "M2FBX"
   configurations { "Debug", "Release" }
   platforms { "Win64" }

project "M2FBX"
   language "C++"
   targetdir "bin/%{cfg.buildcfg}"
   staticruntime "on"
   includedirs { 
   "$(ProjectDir)" ,
   "$(ProjectDir)/Source" ,
   "$(ProjectDir)vendors/fbx/include"
   }

   libdirs {
      "$(ProjectDir)vendors/fbx/lib/vs2019/"
   }

   files { "**.h", "**.c" , "**.cpp", "**.cxx" }

   -- Kind is "ConsoleApp" here so we can debug via exe
   filter "configurations:Debug"
      defines { "DEBUG" }
      kind "ConsoleApp"
      architecture "x86_64"
      symbols "On"

      links {
         "x64/debug/libfbxsdk-mt",
         "x64/debug/libxml2-mt",
         "x64/debug/zlib-mt.lib",
      }

   -- Then package as a functioning DLL for release.
   filter "configurations:Release"
      defines { "NDEBUG" }
      kind "SharedLib"
      architecture "x86_64"
      optimize "On"

      links {
         "x64/release/libfbxsdk-mt",
         "x64/release/libxml2-mt",
         "x64/release/zlib-mt.lib",
      }