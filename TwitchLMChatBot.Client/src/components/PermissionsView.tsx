import {Card, Checkbox, Divider, Flex, Form, Modal, Result,} from "antd";
import {t} from "i18next";

import {useFetchPermissions, useSetPermissions} from "@/hooks/useFetchPermissions.tsx";
import {QuestionCircleOutlined} from "@ant-design/icons";
import {useState} from "react";
import {CheckboxChangeEvent} from "antd/lib/checkbox";
import {PermissionSet} from "@/api/types/permission.tsx";
import {PermissionsHelp} from "@/components/PermissionsHelp.tsx";

type PermissionsViewProps = {
    className?: string
}

export function PermissionsView({className = ""}: PermissionsViewProps): JSX.Element {
    const [showHelp, setShowHelp] = useState(false)

    const [form] = Form.useForm()

    const fetchPermissions = useFetchPermissions()
    const setPermissions = useSetPermissions()

    const {
        unrestricted = true,
        followers = false,
        subscribers = false,
        moderators = false,
        vips = false,
        whiteList = [],
        blackList = []
    } = fetchPermissions.data ?? {}

    function handleUnrestricted(e: CheckboxChangeEvent) {
        if (e.target.checked) {
            form.setFieldsValue({
                followers: false,
                subscribers: false,
                moderators: false,
                vips: false
            })

        }
        form.submit()
    }

    function handleFollowers(e: CheckboxChangeEvent) {
        if (e.target.checked) {
            form.setFieldsValue({
                unrestricted: false,
            })
        }
        form.submit()
    }

    function handleSubs(e: CheckboxChangeEvent) {
        if (e.target.checked) {
            form.setFieldsValue({
                unrestricted: false,
            })
        }
        form.submit()
    }

    function handleMods(e: CheckboxChangeEvent) {
        if (e.target.checked) {
            form.setFieldsValue({
                unrestricted: false,
            })
        }
        form.submit()
    }

    function handleVips(e: CheckboxChangeEvent) {
        if (e.target.checked) {
            form.setFieldsValue({
                unrestricted: false,
            })
        }
        form.submit()
    }

    function handleSubmit(values: PermissionSet) {
        setPermissions.mutate(values, {
            onSuccess: () => fetchPermissions.refetch()
        })
    }

    return <Card className={className} loading={fetchPermissions.isLoading}>
        {fetchPermissions.error && <Result status={'error'} title={fetchPermissions.error.message}/>}
        {fetchPermissions.isSuccess && (<div className=" flex-grow-1">
            <Flex align={'baseline'} gap={4}>
                <div className="text-2xl font-bold">{t("Permissions")}</div>
                <div>
                    <QuestionCircleOutlined onClick={() => setShowHelp(true)}></QuestionCircleOutlined>
                    <Modal open={showHelp} onCancel={() => setShowHelp(false)} footer={null}
                           title={t('Access Control')}>
                        <PermissionsHelp/>
                    </Modal>
                </div>
            </Flex>
            <Form<PermissionSet>
                className={'py-4'} layout={'vertical'}
                form={form}
                initialValues={{
                    unrestricted: unrestricted,
                    followers: followers,
                    subscribers: subscribers,
                    moderators: moderators,
                    vips: vips,
                    whiteList: whiteList,
                    blackList: blackList
                }}
                onFinish={handleSubmit}>
                <Form.Item name={'unrestricted'} noStyle={true} valuePropName={'checked'}>
                    <Checkbox onChange={handleUnrestricted}>Unrestricted</Checkbox>
                </Form.Item>
                <Divider type={'vertical'}/>
                <Form.Item name={'followers'} noStyle={true} valuePropName={'checked'}>
                    <Checkbox onChange={handleFollowers}>Followers</Checkbox>
                </Form.Item>
                <Form.Item name={'subscribers'} noStyle={true} valuePropName={'checked'}>
                    <Checkbox onChange={handleSubs}>Subscribers</Checkbox>
                </Form.Item>
                <Form.Item name={'moderators'} noStyle={true} valuePropName={'checked'}>
                    <Checkbox onChange={handleMods}>Moderators</Checkbox>
                </Form.Item>
                <Form.Item name={'vips'} noStyle={true} valuePropName={'checked'}>
                    <Checkbox onChange={handleVips}>VIPs</Checkbox>
                </Form.Item>

            </Form>
        </div>)}

    </Card>;
}

/*
 <Row>
                    <Col span={12}>
                        <span>{t("Whitelist")}</span> <Button>Add</Button>
                        <List<string> renderItem={(item) => {
                            return <div> {item} <Button danger={true} icon={<CloseOutlined/>}></Button></div>
                        }}></List>
                    </Col>
                    <Col span={12}>
                        <span>{t("Backlist")}</span> <Button>{t("Add")}</Button>
                        <List<string> renderItem={(item) => {
                            return <div> {item} <Button danger={true} icon={<CloseOutlined/>}></Button></div>
                        }}></List>
                    </Col>
                </Row>
 */