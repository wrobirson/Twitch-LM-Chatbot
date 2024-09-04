import {Form, Input, Modal, ModalProps} from "antd";
import {useCreateProvider} from "@/hooks/useCreateProvider.ts";
import {useTranslation} from "react-i18next";

export function LmStudioConfigModal(props: ModalProps & { onCreated: () => void }) {
    const [form] = Form.useForm();
    const {t} = useTranslation();

    const createProvider = useCreateProvider();

    return (
        <Modal
            {...props}
            title={t("LM Studio Configuration")}
            maskClosable={false}
            destroyOnClose
            okButtonProps={{
                loading: createProvider.isPending,
            }}
            cancelButtonProps={{
                disabled: createProvider.isPending,
            }}
            onOk={() => {
                form.submit();
            }}
        >
            <Form
                layout="vertical"
                preserve={false}
                form={form}
                initialValues={{
                    providerType: 0,
                    baseUrl: "http://localhost:1234",
                }}
                onFinish={(values) => {
                    console.log("====================================");
                    console.log(values);
                    console.log("====================================");
                    createProvider.mutate(values, {
                        onSuccess: () => {
                            props.onCreated();
                        },
                    });
                }}
            >
                <Form.Item name={"providerType"} hidden>
                    <Input/>
                </Form.Item>
                <Form.Item
                    name={"providerName"}
                    label={t("Name")}
                    rules={[{required: true}]}
                >
                    <Input/>
                </Form.Item>
                <Form.Item
                    name={"baseUrl"}
                    label={t("Base URL")}
                    rules={[{required: true}]}
                >
                    <Input/>
                </Form.Item>
            </Form>
        </Modal>
    );
}
