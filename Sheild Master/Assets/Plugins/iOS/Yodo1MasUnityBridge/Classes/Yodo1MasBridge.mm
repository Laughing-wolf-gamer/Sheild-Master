//
//  UnityYodo1Mas.mm
//  Pods
//
//  Created by sunmeng on 2020/12/21.
//

#import "Yodo1MasBridge.h"
#import "Yodo1Mas.h"
#import "Yodo1MasUnityTool.h"
#import "Yodo1MasBannerAdView.h"
#import "Yodo1MasBannerHelper.h"
#import "UnityAppController.h"
#import "Yodo1MasBannerAdView+Bridge.h"

static NSString* kYodo1MasGameObject;
static NSString* kYodo1MasMethodName;

@interface Yodo1MasBridge : NSObject <Yodo1MasRewardAdDelegate, Yodo1MasInterstitialAdDelegate, Yodo1MasBannerAdDelegate, Yodo1MasBannerAdViewDelegate>

+ (UIViewController*)getRootViewController;

+ (UIViewController*)topMostViewController:(UIViewController*)controller;

+ (NSString *)stringWithJSONObject:(id)obj error:(NSError**)error;

+ (id)JSONObjectWithString:(NSString*)str error:(NSError**)error;

+ (NSString*)convertToInitJsonString:(int)success error:(NSString*)errorMsg;

+ (NSString*)getSendMessage:(int)flag data:(NSString*)data;

+ (Yodo1MasBridge *)sharedInstance;

- (void)initWithAppId:(NSString *)appId successful:(Yodo1MasInitSuccessful)successful fail:(Yodo1MasInitFail)fail;

- (BOOL)isRewardedAdLoaded;
- (void)showRewardedAd;
- (void)showRewardedAd:(NSString *)placementId;

- (BOOL)isInterstitialAdLoaded;
- (void)showInterstitialAd;
- (void)showInterstitialAd:(NSString *)placementId;

- (BOOL)isBannerAdLoaded;
- (void)showBannerAd;
- (void)showBannerAdWithPlacement:(NSString *)placementId;
- (void)showBannerAdWithAlign:(Yodo1MasAdBannerAlign)align;
- (void)showBannerAdWithAlign:(Yodo1MasAdBannerAlign)align offset:(CGPoint)offset;
- (void)showBannerAdWithPlacement:(NSString *)placement align:(Yodo1MasAdBannerAlign)align offset:(CGPoint)offset;
- (void)dismissBannerAd;
- (void)dismissBannerAdWithDestroy:(BOOL)destroy;

- (void)loadBannerAdV2:(NSString *)param;
- (void)showBannerAdV2:(NSString *)param;
- (void)hideBannerAdV2:(NSString *)param;
- (void)destroyBannerAdV2:(NSString *)param;
- (CGFloat)getBannerWidthInPixels;
- (CGFloat)getBannerHeightInPixels;

@property (nonatomic, strong) NSMutableDictionary<NSString*, Yodo1MasBannerAdView *> *bannerViews;

@end

@implementation Yodo1MasBridge

+ (Yodo1MasBridge *)sharedInstance {
    static Yodo1MasBridge *_instance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _instance = [[Yodo1MasBridge alloc] init];
    });
    return _instance;
}

- (void)initWithAppId:(NSString *)appId successful:(Yodo1MasInitSuccessful)successful fail:(Yodo1MasInitFail)fail {
    [Yodo1Mas sharedInstance].rewardAdDelegate = self;
    [Yodo1Mas sharedInstance].interstitialAdDelegate = self;
    [Yodo1Mas sharedInstance].bannerAdDelegate = self;
    
    _bannerViews = [NSMutableDictionary dictionary];
    
    [[Yodo1Mas sharedInstance] initWithAppKey:appId successful:successful fail:fail];
    
    if (![UIDevice currentDevice].generatesDeviceOrientationNotifications) {
        [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
    }
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(handleDeviceOrientationChange:)
                                         name:UIDeviceOrientationDidChangeNotification object:nil];
}

- (BOOL)isRewardedAdLoaded {
    return [[Yodo1Mas sharedInstance] isRewardAdLoaded];
}
- (void)showRewardedAd {
    [[Yodo1Mas sharedInstance] showRewardAd];
}

- (void)showRewardedAd:(NSString *)placementId {
    [[Yodo1Mas sharedInstance] showRewardAdWithPlacement:placementId];
}

- (BOOL)isInterstitialAdLoaded {
    return [[Yodo1Mas sharedInstance] isInterstitialAdLoaded];
}
- (void)showInterstitialAd {
    [[Yodo1Mas sharedInstance] showInterstitialAd];
}

- (void)showInterstitialAd:(NSString *)placementId {
    [[Yodo1Mas sharedInstance] showInterstitialAdWithPlacement:placementId];
}

- (BOOL)isBannerAdLoaded {
    return [[Yodo1Mas sharedInstance] isBannerAdLoaded];
}
- (void)showBannerAd {
    [[Yodo1Mas sharedInstance] showBannerAd];
}

- (void)showBannerAdWithPlacement:(NSString *)placementId {
    [[Yodo1Mas sharedInstance] showBannerAdWithPlacement:placementId];
}

- (void)showBannerAdWithAlign:(Yodo1MasAdBannerAlign)align {
    [[Yodo1Mas sharedInstance] showBannerAdWithAlign:align];
}

- (void)showBannerAdWithAlign:(Yodo1MasAdBannerAlign)align offset:(CGPoint)offset {
    [[Yodo1Mas sharedInstance] showBannerAdWithAlign:align offset:offset];
}

- (void)showBannerAdWithPlacement:(NSString *)placement align:(Yodo1MasAdBannerAlign)align offset:(CGPoint)offset {
    [[Yodo1Mas sharedInstance] showBannerAdWithPlacement:placement align:align offset:offset];
}

- (void)dismissBannerAd {
    [[Yodo1Mas sharedInstance] dismissBannerAd];
}

- (void)dismissBannerAdWithDestroy:(BOOL)destroy {
    [[Yodo1Mas sharedInstance] dismissBannerAdWithDestroy:destroy];
}

- (Yodo1MasBannerAdView *)getBannerViewFromJson:(NSString *)json needInit:(BOOL)needInit {
    NSError *error = nil;
    id dict = [Yodo1MasBridge JSONObjectWithString:json error:&error];
    if (!dict || error) {
        return nil;
    }
    
    Yodo1MasBridgeBannerAdConfig *config = [Yodo1MasBridgeBannerAdConfig parse:dict];
    if (!config.indexId) {
        return nil;
    }
    Yodo1MasBannerAdView *adView = self.bannerViews[config.indexId];
    if (!adView && needInit) {
        adView = [[Yodo1MasBannerAdView alloc] init];
        adView.yodo1_config = config;
        adView.adDelegate = self;
        [adView setAdSize:config.adSize];
        if (config.adPlacement != nil && config.adPlacement.length > 0) {
            [adView setAdPlacement:config.adPlacement];
        }
        self.bannerViews[config.indexId] = adView;
    }
    return adView;
}

- (void)loadBannerAdV2:(NSString *)jsonString {
    Yodo1MasBannerAdView *adView = [self getBannerViewFromJson:jsonString needInit:YES];

    [GetAppController().rootViewController.view addSubview:adView];
    [adView loadAd];
    [self adjustFrame:adView];
}

- (void)handleDeviceOrientationChange:(NSNotification *)notification {
    UIDeviceOrientation deviceOrientation = [UIDevice currentDevice].orientation;
    switch (deviceOrientation) {
        case UIDeviceOrientationLandscapeLeft:
        case UIDeviceOrientationLandscapeRight:
        case UIDeviceOrientationPortrait:
        case UIDeviceOrientationPortraitUpsideDown:
            [self adjustFrame];
            break;
        default:
            break;
    }
}

- (void)adjustFrame:(Yodo1MasBannerAdView *)adView {
    if (adView == nil) {
        return;
    }
    
    Yodo1MasBridgeBannerAdConfig *config = adView.yodo1_config;
    if (!config) {
        return;
    }
    
    if (config.customPosition.x > 0 || config.customPosition.y > 0) {
        [[Yodo1MasBannerHelper sharedInstance] adjustFrame:adView adPosition:Yodo1MasAdBannerAlignLeft | Yodo1MasAdBannerAlignTop offset:config.customPosition];
    } else {
        [[Yodo1MasBannerHelper sharedInstance] adjustFrame:adView adPosition:config.adPosition offset:CGPointZero];
    }
}

- (void)adjustFrame {
    for (Yodo1MasBannerAdView *adView in self.bannerViews.allValues) {
        [self adjustFrame:adView];
    }
}

- (void)showBannerAdV2:(NSString *)param {
    Yodo1MasBannerAdView* adView = [self getBannerViewFromJson:param needInit:NO];
    if (adView) {
        [adView removeFromSuperview];
        UnityAppController * controller = GetAppController();
        [controller.rootViewController.view addSubview:adView];
        [self adjustFrame:adView];
    }
}

- (void)hideBannerAdV2:(NSString *)param {
    Yodo1MasBannerAdView *adView = [self getBannerViewFromJson:param needInit:NO];
    if (adView) {
        [adView removeFromSuperview];
    }
}

- (void)destroyBannerAdV2:(NSString *)param {
    Yodo1MasBannerAdView *adView = [self getBannerViewFromJson:param needInit:NO];
    if (adView) {
        [adView removeFromSuperview];
        [adView destroy];
        [self.bannerViews removeObjectForKey:adView.yodo1_config.indexId];
    }
    adView = nil;
}

- (CGFloat)getBannerWidth:(int)type {
    return [Yodo1MasBanner sizeFromAdSize:(Yodo1MasBannerAdSize)type].width;
}

- (CGFloat)getBannerHeight:(int)type {
    return [Yodo1MasBanner sizeFromAdSize:(Yodo1MasBannerAdSize)type].height;
}

- (CGFloat)getBannerWidthInPixels:(int)type {
    return [Yodo1MasBanner pixelsFromAdSize:(Yodo1MasBannerAdSize)type].width;
}

- (CGFloat)getBannerHeightInPixels:(int)type {
    return [Yodo1MasBanner pixelsFromAdSize:(Yodo1MasBannerAdSize)type].height;
}

#pragma mark - Banner V2
- (void)onBannerAdLoaded:(Yodo1MasBannerAdView *)banner {
    [self adjustFrame:banner];

    Yodo1MasAdEvent *event = [[Yodo1MasAdEvent alloc] initWithCode:Yodo1MasAdEventCodeLoaded type:Yodo1MasAdTypeBanner];
    if (event == nil) {
        return;
    }
    
    NSString *index = banner.yodo1_config.indexId;
    NSMutableDictionary *dic = (NSMutableDictionary*)event.getJsonObject;
    [dic setObject:index forKey:@"indexId"];
    
    NSString* data = [Yodo1MasBridge stringWithJSONObject:dic error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)onBannerAdFailedToLoad:(Yodo1MasBannerAdView *)banner withError:(Yodo1MasError *)error {
    Yodo1MasAdEvent *event = [[Yodo1MasAdEvent alloc] initWithCode:(Yodo1MasAdEventCode)1004 type:Yodo1MasAdTypeBanner error:error];
    if (event == nil) {
        return;
    }
    NSString* index = banner.yodo1_config.indexId;
    NSMutableDictionary* dic = (NSMutableDictionary*)event.getJsonObject;
    [dic setObject:index forKey:@"indexId"];
    
    NSString* data = [Yodo1MasBridge stringWithJSONObject:dic error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)onBannerAdOpened:(Yodo1MasBannerAdView *)banner {
    Yodo1MasAdEvent *event = [[Yodo1MasAdEvent alloc] initWithCode:Yodo1MasAdEventCodeOpened type:Yodo1MasAdTypeBanner];
    if (event == nil) {
        return;
    }
    NSString* index = banner.yodo1_config.indexId;
    NSMutableDictionary* dic = (NSMutableDictionary*)event.getJsonObject;
    [dic setObject:index forKey:@"indexId"];
    
    NSString* data = [Yodo1MasBridge stringWithJSONObject:dic error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)onBannerAdClosed:(Yodo1MasBannerAdView *)banner {
//    Yodo1MasAdEvent *event = [[Yodo1MasAdEvent alloc] initWithCode:Yodo1MasAdEventCodeClosed type:Yodo1MasAdTypeBanner];
//    if (event == nil) {
//        return;
//    }
//    NSString* data = [Yodo1MasBridge stringWithJSONObject:event.getJsonObject error:nil];
//    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
//    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)onBannerAdFailedToOpen:(Yodo1MasBannerAdView *)banner withError:(Yodo1MasError *)error {
    Yodo1MasAdEvent *event = [[Yodo1MasAdEvent alloc] initWithCode:(Yodo1MasAdEventCode)1005 type:Yodo1MasAdTypeBanner error:error];
    if (event == nil) {
        return;
    }
    NSString *index = banner.yodo1_config.indexId;
    NSMutableDictionary* dic = (NSMutableDictionary*)event.getJsonObject;
    [dic setObject:index forKey:@"indexId"];
    
    NSString* data = [Yodo1MasBridge stringWithJSONObject:dic error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

#pragma mark - Yodo1MasAdDelegate
- (void)onAdOpened:(Yodo1MasAdEvent *)event {
    if (event == nil) {
        return;
    }
    NSString* data = [Yodo1MasBridge stringWithJSONObject:event.getJsonObject error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);

}

- (void)onAdClosed:(Yodo1MasAdEvent *)event {
    if (event == nil) {
        return;
    }
    NSString* data = [Yodo1MasBridge stringWithJSONObject:event.getJsonObject error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

- (void)onAdError:(Yodo1MasAdEvent *)event error:(Yodo1MasError *)error {
    if (event == nil) {
        return;
    }
    NSString* data = [Yodo1MasBridge stringWithJSONObject:event.getJsonObject error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

#pragma mark - Yodo1MasRewardAdvertDelegate
- (void)onAdRewardEarned:(Yodo1MasAdEvent *)event {
    if (event == nil) {
        return;
    }
    NSString* data = [Yodo1MasBridge stringWithJSONObject:event.getJsonObject error:nil];
    NSString* msg = [Yodo1MasBridge getSendMessage:1 data:data];
    UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
}

+ (UIViewController*)getRootViewController {
    UIWindow* window = [[UIApplication sharedApplication] keyWindow];
    if (window.windowLevel != UIWindowLevelNormal) {
        NSArray* windows = [[UIApplication sharedApplication] windows];
        for (UIWindow* _window in windows) {
            if (_window.windowLevel == UIWindowLevelNormal) {
                window = _window;
                break;
            }
        }
    }
    UIViewController* viewController = nil;
    for (UIView* subView in [window subviews]) {
        UIResponder* responder = [subView nextResponder];
        if ([responder isKindOfClass:[UIViewController class]]) {
            viewController = [self topMostViewController:(UIViewController*)responder];
        }
    }
    if (!viewController) {
        viewController = UIApplication.sharedApplication.keyWindow.rootViewController;
    }
    return viewController;
}

+ (UIViewController*)topMostViewController:(UIViewController*)controller {
    BOOL isPresenting = NO;
    do {
        // this path is called only on iOS 6+, so -presentedViewController is fine here.
        UIViewController* presented = [controller presentedViewController];
        isPresenting = presented != nil;
        if (presented != nil) {
            controller = presented;
        }
        
    } while (isPresenting);
    
    return controller;
}

+ (NSString*)stringWithJSONObject:(id)obj error:(NSError**)error {
    if (obj) {
        if (NSClassFromString(@"NSJSONSerialization")) {
            NSData* data = nil;
            @try {
                data = [NSJSONSerialization dataWithJSONObject:obj options:0 error:error];
            }
            @catch (NSException* exception)
            {
                *error = [NSError errorWithDomain:[exception description] code:0 userInfo:nil];
                return nil;
            }
            @finally
            {
            }
            
            if (data) {
                return [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
            }
        }
    }
    return nil;
}

+ (id)JSONObjectWithString:(NSString*)str error:(NSError**)error {
    if (str) {
        if (NSClassFromString(@"NSJSONSerialization")) {
            return [NSJSONSerialization JSONObjectWithData:[str dataUsingEncoding:NSUTF8StringEncoding]
                                                   options:NSJSONReadingAllowFragments
                                                     error:error];
        }
    }
    return nil;
}

+ (NSString*)convertToInitJsonString:(int)success masError:(Yodo1MasError*) error {
    NSMutableDictionary* dict = [NSMutableDictionary dictionary];
    [dict setObject:[NSNumber numberWithInt:success] forKey:@"success"];
    
    if (error != nil) {
        NSString* errorJsonString = [Yodo1MasBridge stringWithJSONObject:[error getJsonObject] error:nil];
        [dict setObject:errorJsonString forKey:@"error"];
    }
    
    NSString* data = [Yodo1MasBridge stringWithJSONObject:dict error:nil];
    return data;
}

+ (NSString*)getSendMessage:(int)flag data:(NSString*)data {
    NSMutableDictionary* dict = [NSMutableDictionary dictionary];
    [dict setObject:[NSNumber numberWithInt:flag] forKey:@"flag"];
    [dict setObject:data forKey:@"data"];
    
    NSError* parseJSONError = nil;
    NSString* msg = [Yodo1MasBridge stringWithJSONObject:dict error:&parseJSONError];
    NSString* jsonError = @"";
    if(parseJSONError){
        jsonError = @"Convert result to json failed!";
    }
    return msg;
}

@end

#pragma mark- ///Unity3d

#ifdef __cplusplus

extern "C" {

#pragma mark - Privacy

void UnityMasSetUserConsent(BOOL consent)
{
    [Yodo1Mas sharedInstance].isGDPRUserConsent = consent;
}

bool UnityMasIsUserConsent()
{
    return [Yodo1Mas sharedInstance].isGDPRUserConsent;
}

void UnityMasSetTagForUnderAgeOfConsent(BOOL isBelowConsentAge)
{
    [Yodo1Mas sharedInstance].isCOPPAAgeRestricted = isBelowConsentAge;
}

bool UnityMasIsTagForUnderAgeOfConsent()
{
    return [Yodo1Mas sharedInstance].isCOPPAAgeRestricted;
}

void UnityMasSetDoNotSell(BOOL doNotSell)
{
    [Yodo1Mas sharedInstance].isCCPADoNotSell = doNotSell;
}

bool UnityMasIsDoNotSell()
{
    return [Yodo1Mas sharedInstance].isCCPADoNotSell;
}

#pragma mark - Initialize

void UnityMasInitWithAppKey(const char* appKey,const char* gameObjectName, const char* callbackMethodName)
{
    NSString* m_appKey = Yodo1MasCreateNSString(appKey);
    NSCAssert(m_appKey != nil, @"AppKey 没有设置!");
    
    NSString* m_gameObject = Yodo1MasCreateNSString(gameObjectName);
    NSCAssert(m_gameObject != nil, @"Unity3d gameObject isn't set!");
    kYodo1MasGameObject = m_gameObject;
    
    NSString* m_methodName = Yodo1MasCreateNSString(callbackMethodName);
    NSCAssert(m_methodName != nil, @"Unity3d callback method isn't set!");
    kYodo1MasMethodName = m_methodName;
    
    [[Yodo1MasBridge sharedInstance] initWithAppId:m_appKey successful:^{
        NSString* data = [Yodo1MasBridge convertToInitJsonString:1 masError:nil];
        NSString* msg = [Yodo1MasBridge getSendMessage:0 data:data];
        UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
    } fail:^(Yodo1MasError * _Nonnull error) {
        NSString* data = [Yodo1MasBridge convertToInitJsonString:0 masError:error];
        NSString* msg = [Yodo1MasBridge getSendMessage:0 data:data];
        UnitySendMessage([kYodo1MasGameObject cStringUsingEncoding:NSUTF8StringEncoding], [kYodo1MasMethodName cStringUsingEncoding:NSUTF8StringEncoding], [msg cStringUsingEncoding:NSUTF8StringEncoding]);
    }];
}

void UnitySetAdBuildConfig(const char * config) {
    NSString * jsonString = Yodo1MasCreateNSString(config);
    NSError * error = nil;
    id dict = [Yodo1MasBridge JSONObjectWithString:jsonString error:&error];
    if (error) {
        return;
    }
    Yodo1MasAdBuildConfig * buildConfig = [Yodo1MasAdBuildConfig instance];
    if (dict[@"enableAdaptiveBanner"]) {
        buildConfig.enableAdaptiveBanner = [dict[@"enableAdaptiveBanner"] boolValue];
    }
    if (dict[@"enableUserPrivacyDialog"]) {
        buildConfig.enableUserPrivacyDialog = [dict[@"enableUserPrivacyDialog"] boolValue];
    }
    if (dict[@"userAgreementUrl"]) {
        NSString* userAgreementUrl = dict[@"userAgreementUrl"];
        if (userAgreementUrl != nil && userAgreementUrl.length > 0) {
            buildConfig.userAgreementUrl = userAgreementUrl;
        }
    }
    if (dict[@"privacyPolicyUrl"]) {
        NSString* privacyPolicyUrl = dict[@"privacyPolicyUrl"];
        if (privacyPolicyUrl != nil && privacyPolicyUrl.length > 0) {
            buildConfig.privacyPolicyUrl = privacyPolicyUrl;
        }
    }
    [[Yodo1Mas sharedInstance] setAdBuildConfig:buildConfig];
}

#pragma mark - Unity Banner

bool UnityIsBannerAdLoaded()
{
    return [[Yodo1MasBridge sharedInstance] isBannerAdLoaded];
}

void UnityShowBannerAd()
{
    [[Yodo1MasBridge sharedInstance] showBannerAd];
}

void UnityShowBannerAdWithPlacement(const char* placementId)
{
    NSString* m_placementId = Yodo1MasCreateNSString(placementId);
    [[Yodo1MasBridge sharedInstance] showBannerAdWithPlacement:m_placementId];
}

void UnityShowBannerAdWithAlign(int align)
{
    [[Yodo1MasBridge sharedInstance] showBannerAdWithAlign:(Yodo1MasAdBannerAlign)align];
}

void UnityShowBannerAdWithAlignAndOffset(int align, int offsetX, int offsetY)
{
    [[Yodo1MasBridge sharedInstance] showBannerAdWithAlign:(Yodo1MasAdBannerAlign)align offset:CGPointMake(offsetX/UIScreen.mainScreen.scale, offsetY/UIScreen.mainScreen.scale)];
}

void UnityShowBannerAdWithPlacementAndAlignAndOffset(const char* placementId, int align, int offsetX, int offsetY)
{
    NSString* m_placementId = Yodo1MasCreateNSString(placementId);
    [[Yodo1MasBridge sharedInstance] showBannerAdWithPlacement:m_placementId align:(Yodo1MasAdBannerAlign)align offset:CGPointMake(offsetX/UIScreen.mainScreen.scale, offsetY/UIScreen.mainScreen.scale)];
}

void UnityDismissBannerAd()
{
    [[Yodo1MasBridge sharedInstance] dismissBannerAd];
}

void UnityDismissBannerAdWithDestroy(bool destroy)
{
    [[Yodo1MasBridge sharedInstance] dismissBannerAdWithDestroy:destroy];
}

#pragma mark - Unity Banner V2
void UnityLoadBannerAdV2(const char* param) {
    NSString* m_param = Yodo1MasCreateNSString(param);
    [[Yodo1MasBridge sharedInstance] loadBannerAdV2:m_param];
}
void UnityShowBannerAdV2(const char* param) {
    NSString* m_param = Yodo1MasCreateNSString(param);
    [[Yodo1MasBridge sharedInstance] showBannerAdV2:m_param];
}
void UnityHideBannerAdV2(const char* param) {
    NSString* m_param = Yodo1MasCreateNSString(param);
    [[Yodo1MasBridge sharedInstance] hideBannerAdV2:m_param];
}
void UnityDestroyBannerAdV2(const char* param) {
    NSString* m_param = Yodo1MasCreateNSString(param);
    [[Yodo1MasBridge sharedInstance] destroyBannerAdV2:m_param];
}

int UnityGetBannerWidthV2(int type) {
    return (int)[[Yodo1MasBridge sharedInstance] getBannerWidth:type];
}

int UnityGetBannerHeightV2(int type) {
    return (int)[[Yodo1MasBridge sharedInstance] getBannerHeight:type];
}

float UnityGetBannerWidthInPixelsV2(int type) {
    float width =  [[Yodo1MasBridge sharedInstance] getBannerWidthInPixels:type];
    return width;
}

float UnityGetBannerHeightInPixelsV2(int type) {
    float height = [[Yodo1MasBridge sharedInstance] getBannerHeightInPixels:type];
    return height;
}

#pragma mark - Unity Interstitial

bool UnityIsInterstitialLoaded()
{
    return [[Yodo1MasBridge sharedInstance] isInterstitialAdLoaded];
}

void UnityShowInterstitialAd()
{
    [[Yodo1MasBridge sharedInstance] showInterstitialAd];
}

void UnityShowInterstitialAdWithPlacementId(const char* placementId)
{
    NSString* m_placementId = Yodo1MasCreateNSString(placementId);
    [[Yodo1MasBridge sharedInstance] showInterstitialAd:m_placementId];
}

#pragma mark - Unity Rewarded Ad

bool UnityIsRewardedAdLoaded()
{
    return [[Yodo1MasBridge sharedInstance] isRewardedAdLoaded];
}

void UnityShowRewardedAd()
{
    [[Yodo1MasBridge sharedInstance] showRewardedAd];
}

void UnityShowRewardedAdWithPlacementId(const char* placementId)
{
    NSString* m_placementId = Yodo1MasCreateNSString(placementId);
    [[Yodo1MasBridge sharedInstance] showRewardedAd:m_placementId];
}

}
#endif
