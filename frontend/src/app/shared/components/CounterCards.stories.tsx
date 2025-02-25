/*
 * Notifo.io
 *
 * @license
 * Copyright (c) Sebastian Stehle. All rights reserved.
 */

import * as React from 'react';
import { ComponentMeta } from '@storybook/react';
import { CounterCards } from './CounterCards';

export default {
    component: CounterCards,
    argTypes: {
        counters: {
            table: {
                disable: true,
            },
        },
    },
} as ComponentMeta<typeof CounterCards>;

const Template = (args: any) => {
    return (
        <CounterCards {...args} />
    );
};

export const Default = Template.bind({});

Default['args'] = {
    counters: {
        notifications_attempt: 400,
        notifications_failed: 20,
        notifications_handled: 100,
        email_attempt: 40000000000,
        email_failed: 20000000,
        email_handled: 100000,
        mobilepush_attempt: 400,
        mobilepush_failed: 20,
        mobilepush_handled: 100,
        messaging_attempt: 400,
        messaging_failed: 0,
        messaging_handled: 100,
        sms_attempt: 0,
        sms_failed: 0,
        sms_handled: 0,
        webhook_attempt: 400,
        webhook_failed: 20,
        webhook_handled: 0,
        webpush_attempt: 0,
        webpush_failed: 20,
        webpush_handled: 100,
    },
};
