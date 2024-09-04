import {Form, Input, Modal, ModalProps, Space} from "antd";
import {useCreatePersonality} from "@/hooks/useCreatePersonality.ts";
import {CreatePersonalityRequest} from "@/api/types/personalities.ts";
import {useTranslation} from "react-i18next";

export function NewPersonalityModal(props: ModalProps & { onSuccess?: () => void }) {
    const [form] = Form.useForm();
    const {t} = useTranslation();

    const {onSuccess} = props;

    const createPersonality = useCreatePersonality();

    function handleSubmit(values: CreatePersonalityRequest) {
        createPersonality.mutate(values, {
            onSuccess
        });
    }

    return (
        <Modal
            {...props}
            title={<Space className="text-xl">{t('Add New Personality')}</Space>}
            onOk={() => form.submit()}
            maskClosable={false}
            destroyOnClose={true}
            okButtonProps={{
                loading: createPersonality.isPending,
            }}
        >
            <Form<CreatePersonalityRequest>
                form={form}
                layout="vertical"
                onFinish={handleSubmit}
                preserve={false}
            >
                <Form.Item
                    name={"personalityName"}
                    label={t("Name")}
                    rules={[{required: true}]}
                >
                    <Input/>
                </Form.Item>

                <Form.Item
                    name={"instructions"}
                    label={t("Instructions")}
                    rules={[{required: true}]}
                >
                    <Input.TextArea/>
                </Form.Item>
            </Form>
        </Modal>
    );
}
