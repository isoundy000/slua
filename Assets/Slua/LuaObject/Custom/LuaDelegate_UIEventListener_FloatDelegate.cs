﻿
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

namespace SLua
{
    public partial class LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out UIEventListener.FloatDelegate ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,-1)) {
				ua=null;
				return op;
			}
            int r = LuaDLL.luaS_checkcallback(l, -1);
			if(r<0) LuaDLL.luaL_error(l,"expect function");
			if(getCacheDelegate<UIEventListener.FloatDelegate>(r,out ua))
				return op;
			LuaDLL.lua_pop(l,1);
            ua = (UnityEngine.GameObject a1,float a2) =>
            {
                int error = pushTry(l);
                LuaDLL.lua_getref(l, r);

				pushValue(l,a1);
				pushValue(l,a2);
				if (LuaDLL.lua_pcall(l, 2, -1, error) != 0) {
					LuaDLL.lua_pop(l, 1);
				}
				LuaDLL.lua_pop(l, 1);
			};
			cacheDelegate(r,ua);
			return op;
		}
	}
}
