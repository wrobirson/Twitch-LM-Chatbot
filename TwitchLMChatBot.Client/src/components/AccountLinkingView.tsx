import {useQueryClient} from "@tanstack/react-query";
import {Avatar, Button, Card, Flex, Popconfirm, Result, Space, } from "antd";
import {BsTwitch} from "react-icons/bs";
import {useFetchBroadcasterAccount} from "@/hooks/useFetchBroadcasterAccount.ts";
import {useDeleteBroadcasterAccount} from "@/hooks/useDeleteBroadcasterAccount.ts";
import env from "@/env.ts";
import {useTranslation} from "react-i18next";

export default function AccountLinkingView() {
    const {t} = useTranslation();
    const queryClient = useQueryClient();
    const accountQuery = useFetchBroadcasterAccount();
    const unlinkAccount = useDeleteBroadcasterAccount();

    const handleLinkAccount = async () => {
        location.href = `${env.API_BASE_URL}/auth/twitch?accountType=0`;
    };

    const handleUnlinkAccount = () => {
        unlinkAccount.mutate(undefined, {
            onSuccess: () => {
                queryClient.invalidateQueries(accountQuery);
                accountQuery.refetch();
            },
        });
    };

    const {displayName, broadcasterType, profileImageUrl} =
    accountQuery.data || {};

    return (
        <Card loading={accountQuery.isLoading}>
            <div className="text-2xl font-bold">{t('Broadcaster Account')}</div>
            {accountQuery.error && <Result status={'error'} title={accountQuery.error.message}/>}
            {accountQuery.isSuccess && accountQuery.data && (
                <>
                    <div>{t('Manage your linked Twitch account')}</div>
                    <div className="py-6">
                        <Flex gap={6}>
                            <Avatar src={profileImageUrl} size={64}/>
                            <div>
                                <div className="text-lg font-medium">{displayName}</div>
                                <p className="text-sm text-muted-foreground">
                                    {broadcasterType}
                                </p>
                            </div>
                        </Flex>
                    </div>
                    <div className="text-center">
                        <Popconfirm
                            title={t('Are you sure you want to unlink the account?')}
                            onConfirm={handleUnlinkAccount}
                        >
                            <Button type="primary" danger loading={unlinkAccount.isPending}>
                                {t('Unlink account')}
                            </Button>
                        </Popconfirm>
                    </div>
                </>
            )}
            {accountQuery.isSuccess && !accountQuery.data  && (
                <>
                    <div>{t('Link your Twitch account to get started')}</div>
                    <div className="py-11">{t('You do not have a linked Twitch account.')}</div>
                    <div className="text-center">
                        <Button type="primary" onClick={handleLinkAccount}>
                            <Space size={"small"}>
                                <BsTwitch className="h-4 w-4"/>
                                {t('Link account')}
                            </Space>
                        </Button>
                    </div>
                </>
            )}

        </Card>
    );
}
