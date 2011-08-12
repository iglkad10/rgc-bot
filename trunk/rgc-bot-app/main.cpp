#include "../rgc-bot/rgc-bot.h"
#include <Windows.h>

int main()
{
	HMODULE hRgc = LoadLibrary(L"rgc-bot.dll");

	CREATEINTERFACEFUNC createfn = (CREATEINTERFACEFUNC) GetProcAddress(hRgc, "CreateInterface");
	DELETEINTERFACEFUNC deletefn = (DELETEINTERFACEFUNC) GetProcAddress(hRgc, "DeleteInterface");

	IRgcInterface* interf = createfn();

	interf->Init();
	interf->Connect("Ro.Community", "ytinummoc.or");
	interf->Uninit();

	deletefn(interf);
	FreeLibrary(hRgc);
}