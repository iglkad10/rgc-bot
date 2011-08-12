#pragma once

#ifdef RGCBOT_EXPORTS
#define RGCBOT_API extern "C" __declspec(dllexport)
#else
#define RGCBOT_API extern "C" __declspec(dllimport)
#endif

#define RGC_HOSTNAME	"94.23.34.55"
#define RGC_PORT		45493

class IRgcInterface
{
public:
	virtual bool Init() = 0;
	virtual void Uninit() = 0;
	virtual bool Connect(const char* username, const char* password) = 0;
};

typedef IRgcInterface* (*CREATEINTERFACEFUNC)();
typedef void (*DELETEINTERFACEFUNC)(IRgcInterface*);

RGCBOT_API
IRgcInterface* CreateInterface();

RGCBOT_API
void DeleteInterface(IRgcInterface*);