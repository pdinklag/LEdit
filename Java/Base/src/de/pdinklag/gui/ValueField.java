package de.pdinklag.gui;

import javax.swing.*;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import java.awt.*;

/**
 * Abstract base class for text fields only accepting certain formats.
 *
 * @param <T> The type of the value represented / edited by this text field.
 */
public abstract class ValueField<T> extends JTextField
        implements DocumentListener {

    private static final long serialVersionUID = 3363071098772453234L;
    private static final Color INVALID_VALUE_COLOR = new Color(255, 192, 192);

    private transient boolean ignoreEvents = false;
    private transient Color defaultBackgroundColor;
    private T value;

    public ValueField(T value) {
        super(value.toString());
        defaultBackgroundColor = getBackground();

        getDocument().addDocumentListener(this);
    }

    /**
     * Parses a text to the value represented by it.
     * <p/>
     * The text should be in the same format as if it was constructed by the represented value's
     * {@link Object#toString()} method.
     *
     * @param text The text to parse.
     * @return The value represented by the text, or <tt>null</tt> if the text could not be parsed.
     */
    protected abstract T parse(String text);

    public void setValue(T value) {
        this.value = value;

        ignoreEvents = true;
        setText((value == null) ? "" : value.toString());
        ignoreEvents = false;
    }

    /**
     * Gets the current value that this field's text represents.
     *
     * @return The value represented by the text, or <tt>null</tt> if the text is invalid.
     */
    public T getValue() {
        return value;
    }

    /**
     * Tests whether the current text is valid.
     *
     * @return <tt>true</tt> if the current text is valid, <tt>false</tt> otherwise.
     */
    public boolean isValid() {
        return (value != null);
    }

    private void updateValue() {
        if (!ignoreEvents) {
            value = parse(getText());
            super.setBackground((value == null) ? INVALID_VALUE_COLOR : defaultBackgroundColor);
        }
    }

    @Override
    public void insertUpdate(DocumentEvent e) {
        updateValue();
    }

    @Override
    public void removeUpdate(DocumentEvent e) {
        updateValue();
    }

    @Override
    public void changedUpdate(DocumentEvent e) {
        updateValue();
    }

    @Override
    public void setBackground(Color color) {
        super.setBackground(color);
        defaultBackgroundColor = getBackground();
    }
}
